#!/usr/bin/env python3
# -*- coding: utf-8 -*-

import os
import sys

def extract_pdf_content(pdf_path):
    """提取PDF內容的詳細方法"""
    
    print(f"正在處理檔案: {pdf_path}")
    print(f"檔案大小: {os.path.getsize(pdf_path)} bytes")
    
    # 方法1: 使用PyPDF2
    try:
        import PyPDF2
        print("嘗試使用PyPDF2...")
        
        with open(pdf_path, 'rb') as file:
            pdf_reader = PyPDF2.PdfReader(file)
            print(f"總頁數: {len(pdf_reader.pages)}")
            
            all_text = ""
            for page_num, page in enumerate(pdf_reader.pages):
                text = page.extract_text()
                print(f"第{page_num + 1}頁文字長度: {len(text) if text else 0}")
                if text:
                    all_text += f"=== 第{page_num + 1}頁 ===\n{text}\n\n"
                    
            if all_text:
                print("成功提取文字內容:")
                print("-" * 50)
                print(all_text)
                return all_text
            else:
                print("PyPDF2無法提取文字內容")
                
    except ImportError:
        print("PyPDF2未安裝")
    except Exception as e:
        print(f"PyPDF2錯誤: {e}")
    
    # 方法2: 嘗試讀取原始內容
    print("\n嘗試讀取原始PDF內容...")
    try:
        with open(pdf_path, 'rb') as file:
            content = file.read()
            print(f"原始內容長度: {len(content)} bytes")
            
            # 嘗試尋找可能的文字內容
            try:
                text_content = content.decode('utf-8', errors='ignore')
                print("部分可讀取內容:")
                print("-" * 30)
                # 只顯示前1000個字符
                print(text_content[:1000])
                print("\n...")
                print(text_content[-500:])  # 顯示最後500個字符
                
                return text_content
            except Exception as e:
                print(f"無法解碼內容: {e}")
                
    except Exception as e:
        print(f"讀取檔案錯誤: {e}")
    
    return None

def main():
    pdf_path = "GLATEST/11312損益表-月份.pdf"
    
    if not os.path.exists(pdf_path):
        print(f"檔案不存在: {pdf_path}")
        return
    
    content = extract_pdf_content(pdf_path)
    
    if content:
        # 儲存提取的內容
        output_file = "extracted_content.txt"
        with open(output_file, 'w', encoding='utf-8') as f:
            f.write(content)
        print(f"\n內容已儲存至: {output_file}")
    else:
        print("無法提取PDF內容")

if __name__ == "__main__":
    main() 