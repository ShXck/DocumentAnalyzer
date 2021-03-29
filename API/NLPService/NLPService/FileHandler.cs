﻿using System.IO;
using System.Text;
using Spire.Pdf;
using Spire.Doc;


namespace NLPService
{
    public class FileHandler
    {
        public static string GetTextFromPDF(string file_path)
        {
            // Creates a new pdf document object
            PdfDocument document = new PdfDocument();
            // Load the document from the file path
            document.LoadFromFile(file_path);
            // Creates a string with all the text from the pdf file
            StringBuilder text = new StringBuilder();
            foreach (PdfPageBase page in document.Pages)
            {
                text.Append(page.ExtractText());
            }
            return text.ToString();
        }
      
        public static string GetTextFromWord(string file_path)
        {
            // Creates a new word document object
            Document document = new Document();
            // Load the document from the file path
            document.LoadFromFile(file_path);
            // Creates a string with all the text from the word file
            string text = document.GetText();
            return text;
        }
  
        public static string GetTextFromTxt(string file_path)
        {   
            // Creates a strubg with all text from the plain text file
            string text = System.IO.File.ReadAllText(file_path);
            return text.ToString();
        }

        public static string GetBlobText(string file_path)
        {
            // Obtain the extension of the file
            string extension = Path.GetExtension(file_path);
            
            // Obtain the text from the pdf file
            if (extension.Equals(".pdf"))
            {
                string text = GetTextFromPDF(file_path);
                return text;
            }
            // Obtain the text from the word file
            else if (extension.Equals(".docx"))
            {
                string text = GetTextFromWord(file_path);
                return text;
            }
            // Obtaib the text from the plain text 
            else if (extension.Equals(".txt"))
            {
                string text = GetTextFromTxt(file_path);
                return text;
            }
            else
            {
                // The format of the file is not a pdf, word or plain text
                string text = "El formato del documento no es soportado";
                return text;
            }
        }
    }
}
