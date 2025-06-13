#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
高級PDF內容提取工具
嘗試多種方法提取PDF中的實際數據
"""

import os
import re
import sys

def extract_with_pypdf2(pdf_path):
    """使用PyPDF2提取"""
    try:
        import PyPDF2
        print("使用PyPDF2提取...")
        
        with open(pdf_path, 'rb') as file:
            reader = PyPDF2.PdfReader(file)
            text = ""
            for page in reader.pages:
                page_text = page.extract_text()
                if page_text:
                    text += page_text + "\n"
            return text if text.strip() else None
    except Exception as e:
        print(f"PyPDF2提取失敗: {e}")
        return None

def extract_with_pdfplumber(pdf_path):
    """使用pdfplumber提取（如果可用）"""
    try:
        import pdfplumber
        print("使用pdfplumber提取...")
        
        text = ""
        with pdfplumber.open(pdf_path) as pdf:
            for page in pdf.pages:
                page_text = page.extract_text()
                if page_text:
                    text += page_text + "\n"
                    
                # 嘗試提取表格
                tables = page.extract_tables()
                if tables:
                    for table in tables:
                        for row in table:
                            if row:
                                text += " | ".join([str(cell) if cell else "" for cell in row]) + "\n"
        return text if text.strip() else None
    except Exception as e:
        print(f"pdfplumber提取失敗: {e}")
        return None

def extract_raw_text(pdf_path):
    """提取原始文字內容"""
    try:
        print("嘗試原始文字提取...")
        with open(pdf_path, 'rb') as file:
            content = file.read()
            
        # 尋找可能的文字模式
        text_patterns = []
        
        # 模式1: 尋找中文和數字
        chinese_pattern = re.compile(r'[\u4e00-\u9fff]+')
        number_pattern = re.compile(r'\d+[,\d]*\.?\d*')
        
        # 轉換為字符串並搜索
        try:
            content_str = content.decode('utf-8', errors='ignore')
            chinese_matches = chinese_pattern.findall(content_str)
            number_matches = number_pattern.findall(content_str)
            
            if chinese_matches or number_matches:
                text_patterns.extend(chinese_matches)
                text_patterns.extend(number_matches)
        except:
            pass
        
        # 模式2: 尋找常見會計詞彙的byte模式
        accounting_terms = [
            '營業收入', '營業成本', '營業費用', '營業利益', '營業外',
            '稅前', '稅後', '淨利', '每股', '盈餘', '損益', '資產',
            '負債', '股東權益', '現金', '應收', '應付'
        ]
        
        for term in accounting_terms:
            try:
                term_bytes = term.encode('utf-8')
                if term_bytes in content:
                    text_patterns.append(f"找到關鍵字: {term}")
            except:
                continue
        
        # 模式3: 尋找數字模式
        # 在PDF中尋找可能的財務數字格式
        content_hex = content.hex()
        
        # 常見的中文字元在UTF-8中的模式
        chinese_hex_patterns = [
            'e596b6e6a5ad',  # 營業
            'e694b6e585a5',  # 收入
            'e68890e69cac',  # 成本
            'e8b2bbe794a8',  # 費用
            'e588a9e79b8a',  # 利益
        ]
        
        found_terms = []
        for pattern in chinese_hex_patterns:
            if pattern in content_hex:
                found_terms.append(f"發現中文模式: {pattern}")
        
        if text_patterns or found_terms:
            result = "=== 提取的內容模式 ===\n"
            result += "\n".join(text_patterns[:50])  # 限制輸出
            result += "\n\n=== 發現的編碼模式 ===\n"
            result += "\n".join(found_terms)
            return result
            
        return None
        
    except Exception as e:
        print(f"原始提取失敗: {e}")
        return None

def analyze_pdf_structure(pdf_path):
    """分析PDF結構"""
    try:
        print("分析PDF結構...")
        with open(pdf_path, 'rb') as file:
            content = file.read()
            
        # 查找PDF對象
        stream_count = content.count(b'stream')
        endstream_count = content.count(b'endstream')
        obj_count = content.count(b'obj')
        
        # 查找字體信息
        font_count = content.count(b'/Font')
        fontname_count = content.count(b'/FontName')
        
        # 查找編碼信息
        encoding_count = content.count(b'/Encoding')
        
        analysis = f"""
PDF結構分析:
- 檔案大小: {len(content)} bytes
- Stream對象: {stream_count}
- Font對象: {font_count}
- 編碼對象: {encoding_count}
- 總對象數: {obj_count}
        """
        
        return analysis
        
    except Exception as e:
        print(f"結構分析失敗: {e}")
        return None

def create_markdown_from_extracted_content(content, pdf_filename):
    """從提取的內容創建Markdown"""
    if not content:
        return create_empty_template(pdf_filename)
    
    # 基本清理和格式化
    lines = content.split('\n')
    cleaned_lines = [line.strip() for line in lines if line.strip()]
    
    markdown = f"# {pdf_filename}\n\n"
    markdown += "## 提取的PDF內容\n\n"
    
    # 嘗試識別和組織內容
    current_section = ""
    in_table = False
    
    for line in cleaned_lines:
        # 檢測可能的表格數據
        if '|' in line or re.search(r'\d+[,\d]*\.?\d*', line):
            if not in_table:
                markdown += "\n### 數據內容\n\n"
                markdown += "| 項目 | 數值 |\n"
                markdown += "|------|------|\n"
                in_table = True
            
            # 處理表格行
            if '|' in line:
                markdown += f"{line}\n"
            else:
                # 嘗試解析為表格
                parts = re.split(r'[:\s]+', line, 1)
                if len(parts) == 2:
                    markdown += f"| {parts[0]} | {parts[1]} |\n"
                else:
                    markdown += f"| {line} | |\n"
        else:
            if in_table:
                markdown += "\n"
                in_table = False
            
            # 檢測標題
            if any(keyword in line for keyword in ['營業', '收入', '成本', '費用', '利益', '損益']):
                markdown += f"\n## {line}\n\n"
            else:
                markdown += f"{line}\n\n"
    
    return markdown

def create_empty_template(filename):
    """創建空模板"""
    return f"""# {filename}

## 無法提取PDF內容

由於PDF格式或編碼問題，無法自動提取文字內容。

### 建議解決方案：

1. **使用專業PDF工具**
   - Adobe Acrobat
   - PDF-XChange Editor
   - Foxit Reader

2. **線上轉換工具**
   - PDF24
   - SmallPDF
   - ILovePDF

3. **手動輸入**
   - 查看PDF內容
   - 手動填入以下模板

---

## 損益表模板

### 基本資訊
- **期間**: 
- **單位**: 
- **製表日期**: 

### 營業收入
| 項目 | 金額 |
|------|------|
| 營業收入 | |
| 營業收入淨額 | |

### 營業成本與費用
| 項目 | 金額 |
|------|------|
| 營業成本 | |
| 營業費用 | |

### 損益計算
| 項目 | 金額 |
|------|------|
| 營業利益 | |
| 稅前淨利 | |
| 稅後淨利 | |

---
*請手動填入PDF中的實際數據*
"""

def main():
    pdf_path = "GLATEST/11312損益表-月份.pdf"
    
    if not os.path.exists(pdf_path):
        print(f"檔案不存在: {pdf_path}")
        return
    
    print(f"處理檔案: {pdf_path}")
    
    # 分析PDF結構
    structure_info = analyze_pdf_structure(pdf_path)
    if structure_info:
        print(structure_info)
    
    # 嘗試多種提取方法
    extracted_content = None
    
    # 方法1: PyPDF2
    if not extracted_content:
        extracted_content = extract_with_pypdf2(pdf_path)
    
    # 方法2: pdfplumber
    if not extracted_content:
        extracted_content = extract_with_pdfplumber(pdf_path)
    
    # 方法3: 原始文字提取
    if not extracted_content:
        extracted_content = extract_raw_text(pdf_path)
    
    # 創建Markdown文件
    filename = os.path.splitext(os.path.basename(pdf_path))[0]
    markdown_content = create_markdown_from_extracted_content(extracted_content, filename)
    
    # 保存結果
    output_path = f"{filename}_extracted.md"
    try:
        with open(output_path, 'w', encoding='utf-8') as f:
            f.write(markdown_content)
        print(f"\nMarkdown檔案已保存: {output_path}")
        
        if extracted_content:
            print("\n成功提取PDF內容!")
            print("預覽內容:")
            print("-" * 50)
            print(extracted_content[:500] + "..." if len(extracted_content) > 500 else extracted_content)
        else:
            print("\n無法提取PDF內容，已創建空模板供手動填入。")
            
    except Exception as e:
        print(f"保存檔案失敗: {e}")
        print("\nMarkdown內容:")
        print(markdown_content)

if __name__ == "__main__":
    main() 