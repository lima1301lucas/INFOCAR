using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace LePDF
{
    class Program
    {
        static void Main(string[] args)
        {
            string dirEdital = @"Z:\leilao_2022\05_Maio\02 segunda-feira\Sergio Villa Nova de Freitas-5235\Documentos\edital_5235_1.pdf";
            string textoPdf = ExtraiTexto(dirEdital);

            var lote = "";
            var placa = "";
            var cor = "";
            var ano = "";
            var modelo = "";

            var rgxLote = new Regex(@"LOTE\s+\d+");
            var rgxPlaca = new Regex(@"placa\s+(...)+[0-9]{4}|placa\s+[A-Z]{3}-[0-9]{4}|placa\s+(...)+[0-9]{3}");
            var rgxCor = new Regex(@"cor\s+[a-z]+");
            var rgxano = new Regex(@"modelo\s+\d{4}");
            var rgxmodelo = new Regex(@"LOTE\s+\d+:\s+[^,]+");

            Match rgxLoteRep = rgxLote.Match(textoPdf);
            Match rgxPlacaRep = rgxPlaca.Match(textoPdf);
            Match rgxCorRep = rgxCor.Match(textoPdf);
            Match rgxanoRep = rgxano.Match(textoPdf);
            Match rgxmodeloRep = rgxmodelo.Match(textoPdf);

            while (rgxLoteRep.Success)
            {
                if (rgxLoteRep.Success)
                {
                    lote = rgxLoteRep.ToString().Replace("LOTE ", "");
                }
                if (rgxPlacaRep.Success)
                {
                    placa = rgxPlacaRep.ToString().Replace("placa ", "");
                }
                if (rgxCorRep.Success)
                {
                    cor = rgxCorRep.ToString().Replace("cor ", "");
                }
                if (rgxanoRep.Success)
                {
                    ano = rgxanoRep.ToString().Replace("modelo ", "");
                }
                if (rgxmodeloRep.Success)
                {
                    modelo = Regex.Replace(rgxmodeloRep.ToString(), @"LOTE\s+\d+:\s+", "");
                }
            }
            Console.WriteLine(lote + placa + cor + ano + modelo + "\n");
            Console.ReadKey();
        }

        static string ExtraiTexto(string nomeDoArquivo)
        {
            string retorno = "";
            PdfReader pdfReader = new PdfReader(nomeDoArquivo);
            PdfDocument pdfDoc = new PdfDocument(pdfReader);

            for(int page = 1; page<=pdfDoc.GetNumberOfPages(); page++)
            {
                ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                string conteudo = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(page), strategy);
                retorno += conteudo;
            }
            pdfDoc.Close();
            pdfReader.Close();
            return retorno;

        }
    }
}
