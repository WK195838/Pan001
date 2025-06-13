#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
PDF to Markdown Converter
將PDF檔案轉換為Markdown格式
"""

import sys
import os

def extract_pdf_text_simple(pdf_path):
    """
    簡單的PDF文字提取方法
    由於環境限制，這裡提供基本的框架
    """
    try:
        # 嘗試使用不同的PDF處理方法
        
        # 方法1: 嘗試使用PyPDF2
        try:
            import PyPDF2
            with open(pdf_path, 'rb') as file:
                pdf_reader = PyPDF2.PdfReader(file)
                text = ""
                for page in pdf_reader.pages:
                    text += page.extract_text() + "\n"
                return text
        except ImportError:
            print("PyPDF2 未安裝")
            
        # 方法2: 嘗試使用pdfplumber
        try:
            import pdfplumber
            text = ""
            with pdfplumber.open(pdf_path) as pdf:
                for page in pdf.pages:
                    page_text = page.extract_text()
                    if page_text:
                        text += page_text + "\n"
            return text
        except ImportError:
            print("pdfplumber 未安裝")
            
        # 方法3: 嘗試使用pymupdf (fitz)
        try:
            import fitz
            doc = fitz.open(pdf_path)
            text = ""
            for page in doc:
                text += page.get_text() + "\n"
            doc.close()
            return text
        except ImportError:
            print("pymupdf 未安裝")
            
        return None
        
    except Exception as e:
        print(f"錯誤: {e}")
        return None

def convert_to_markdown(text, title="損益表"):
    """
    將提取的文字轉換為Markdown格式
    """
    if not text:
        return create_template(title)
    
    # 基本的文字處理
    lines = text.split('\n')
    markdown = f"# {title}\n\n"
    
    current_section = ""
    for line in lines:
        line = line.strip()
        if not line:
            continue
            
        # 檢測可能的表格或數據
        if any(keyword in line for keyword in ["收入", "支出", "費用", "利益", "損失", "金額"]):
            if current_section != "表格":
                markdown += "\n## 財務資料\n\n"
                markdown += "| 項目 | 金額 |\n"
                markdown += "|------|------|\n"
                current_section = "表格"
            
            # 嘗試解析表格行
            parts = line.split()
            if len(parts) >= 2:
                item = parts[0]
                amount = ' '.join(parts[1:])
                markdown += f"| {item} | {amount} |\n"
            else:
                markdown += f"| {line} | |\n"
        else:
            if current_section == "表格":
                markdown += "\n"
                current_section = ""
            markdown += f"{line}\n\n"
    
    return markdown

def create_template(title="損益表"):
    """
    創建損益表模板
    """
    return f"""# {title}

## 基本資訊
- **期間**: 
- **單位**: 新台幣元
- **製表日期**: 

## 營業收入
| 項目 | 本期金額 | 累計金額 |
|------|----------|----------|
| 營業收入 | | |
| 營業收入淨額 | | |

## 營業成本及費用
| 項目 | 本期金額 | 累計金額 |
|------|----------|----------|
| 營業成本 | | |
| 營業費用 | | |

## 損益計算
| 項目 | 本期金額 | 累計金額 |
|------|----------|----------|
| 營業利益 | | |
| 營業外損益 | | |
| 稅前淨利 | | |
| 所得稅 | | |
| 稅後淨利 | | |

---
*請填入實際數據*
"""

def main():
    if len(sys.argv) < 2:
        print("使用方法: python pdf_to_md.py <PDF檔案路徑>")
        pdf_path = "GLATEST/11312損益表-月份.pdf"
        print(f"嘗試處理預設檔案: {pdf_path}")
    else:
        pdf_path = sys.argv[1]
    
    if not os.path.exists(pdf_path):
        print(f"檔案不存在: {pdf_path}")
        return
    
    print(f"正在處理: {pdf_path}")
    
    # 提取PDF文字
    text = extract_pdf_text_simple(pdf_path)
    
    # 轉換為Markdown
    title = os.path.splitext(os.path.basename(pdf_path))[0]
    markdown_content = convert_to_markdown(text, title)
    
    # 儲存Markdown檔案
    output_path = f"{title}.md"
    try:
        with open(output_path, 'w', encoding='utf-8') as f:
            f.write(markdown_content)
        print(f"Markdown檔案已儲存: {output_path}")
    except Exception as e:
        print(f"儲存檔案時發生錯誤: {e}")
        print("Markdown內容:")
        print(markdown_content)

if __name__ == "__main__":
    main() 