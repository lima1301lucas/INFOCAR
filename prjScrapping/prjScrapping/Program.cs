using System;
using System.IO;
using System.Text.RegularExpressions;

namespace prjScrapping
{
    class Program
    {
        static void Main(string[] args)
        {
            string leiloeiro = "Joao Emilio de Oliveira Filho";
            string id = "2812";
            string data = "08042022";

            string arqCSV = @"C:\Users\lima1\Desktop\Joao Emilio de Oliveira Filho-2812\Joao Emilio de Oliveira Filho-2812\" + leiloeiro + "-" + id + "@" + data + "-teste.txt";
           
            try
            {
                if (!File.Exists(arqCSV))
                {
                    using (FileStream fsCriaCsv = File.Create(arqCSV))
                    {
                        fsCriaCsv.Close();
                    }

                    if (File.Exists(arqCSV))
                    {
                        using (StreamWriter fsEscCsv = File.AppendText(arqCSV))
                        {
                            fsEscCsv.WriteLine("LOTE;ANO;COR;COMITENTE");

                            string[] files = System.IO.Directory.GetFiles(@"C:\Users\lima1\Desktop\Joao Emilio de Oliveira Filho-2812\Joao Emilio de Oliveira Filho-2812\20220408", "lote*.html");

                            foreach (String file in files)
                            {
                                using var dirLotes = new StreamReader(file);
                                string arquivo;

                                var lote = "";
                                var ano = "";
                                var cor = "";
                                var comitente = "";

                                while ((arquivo = dirLotes.ReadLine()) != null)
                                {
                                    var rgxLote = new Regex(@"selected>LOTE\s+\d+");
                                    var rgxAno = new Regex(@"Ano/Modelo:</b>\s+\d+");
                                    var rgxCor = new Regex(@"(Cor:</b>)+(...)+\s(</div>)");
                                    var rgxComitente = new Regex(@"(Comitente:</b>)+(?:...)+([</p>]\w)");

                                    Match rgxLoteRep = rgxLote.Match(arquivo);
                                    Match rgxAnoRep = rgxAno.Match(arquivo);
                                    Match rgxCorRep = rgxCor.Match(arquivo);
                                    Match rgxComitenteRep = rgxComitente.Match(arquivo);

                                    if (rgxLoteRep.Success)
                                    {
                                        lote = rgxLoteRep.ToString().Replace("selected>LOTE ", "");                                        
                                    }
                                    if (rgxAnoRep.Success)
                                    {
                                        ano = rgxAnoRep.ToString().Replace("Ano/Modelo:</b> ", "");
                                    }
                                    if (rgxCorRep.Success)
                                    {
                                        cor = rgxCorRep.ToString().Replace("Cor:</b> ", "");
                                        cor = cor.Replace(" </div>", "");
                                    }  
                                    if (rgxComitenteRep.Success)
                                    {
                                        comitente = rgxComitenteRep.ToString().Replace("Comitente:</b> ", "");
                                    }

                                }
                                fsEscCsv.Write(lote.ToString() + ";" + ano.ToString() + ";" + cor.ToString() + ";" + comitente.ToString() + "\n");

                                dirLotes.Close();
                            }

                        }
                    }
                    else Console.WriteLine("Linha não escrita");
                }
                else Console.WriteLine("Arquivo já existe");
            }
            catch
            {
                Console.WriteLine("Erro inesperado!");
            }
        }
    }
}