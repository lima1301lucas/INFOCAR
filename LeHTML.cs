using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LeHTML
{
    class Program
    {
        static void Main(string[] args)
        {
            string leiloeiro = "Sergio Villa Nova de Freitas";
            string id = "5235";
            string data = "02052022";

            string arqCSV = @"Z:\leilao_2022\05_Maio\02 segunda-feira\Sergio Villa Nova de Freitas-5235\" + leiloeiro + "-" + id + "@" + data + "-TesteTI.csv";

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
                            fsEscCsv.WriteLine("LOTE;PLACA;MODELO;ANO;COR");

                            string[] files = System.IO.Directory.GetFiles(@"Z:\leilao_2022\05_Maio\02 segunda-feira\Sergio Villa Nova de Freitas-5235\20220502", "lote*.html");

                            foreach (String file in files)
                            {
                                using (StreamReader dirLotes = new StreamReader(file))
                                {
                                    string arquivo;
                                    var lote = "";
                                    var placa = "";
                                    var cor = "";
                                    var ano = "";
                                    var modelo = "";

                                    while ((arquivo = dirLotes.ReadLine()) != null)
                                    {
                                        var rgxLote = new Regex(@"LOTE\s+\d+");
                                        var rgxPlaca = new Regex(@"placa\s+(...)+[0-9]{4}|placa\s+[A-Z]{3}-[0-9]{4}|placa\s+(...)+[0-9]{3}");
                                        var rgxCor = new Regex(@"cor\s+[a-z]+");
                                        var rgxano = new Regex(@"modelo\s+\d{4}");
                                        var rgxmodelo = new Regex(@"LOTE\s+\d+:\s+[^,]+");

                                        Match rgxLoteRep = rgxLote.Match(arquivo);
                                        Match rgxPlacaRep = rgxPlaca.Match(arquivo);
                                        Match rgxCorRep = rgxCor.Match(arquivo);
                                        Match rgxanoRep = rgxano.Match(arquivo);
                                        Match rgxmodeloRep = rgxmodelo.Match(arquivo);

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
                                            //modelo = rgxmodeloRep.ToString().Replace("", "");
                                            modelo = Regex.Replace(rgxmodeloRep.ToString(), @"LOTE\s+\d+:\s+", "");
                                        }
                                    }
                                    fsEscCsv.Write(lote.ToString() + ";" + placa.ToString() + ";" + modelo.ToString() + ";" + ano.ToString() + ";" + cor.ToString() + "\n");

                                    dirLotes.Close();
                                }
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
