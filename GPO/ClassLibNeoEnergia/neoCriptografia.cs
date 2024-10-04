using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Neocriptografia;



/// <summary>
/// Classe neoCriptografia 
/// Objetivo: Retornar String de Conexão para App
/// Criada em 17/02/2009 por Aline Pinheiro
/// </summary>
/// 

namespace CELPE.CRIPTO
{
    public class neoSchemasOracle
    {
        //Captura das Linhas do .txt
        private ArrayList LerTxt()
        {
            ArrayList linhasTxt = new ArrayList();
            string sLine = "";

            if (File.Exists("d:\\data\\app_schemas_oracle.txt"))
            {
                //Captura o arquivo .txt 
                StreamReader objReaderd = new StreamReader("d:\\data\\app_schemas_oracle.txt");
                //Diretório D:/
                while (sLine != null)
                {
                    sLine = objReaderd.ReadLine();
                    if (sLine != null)
                        linhasTxt.Add(sLine);
                }
                objReaderd.Close();

            }

            if (File.Exists("c:\\data\\app_schemas_oracle.txt"))
            {
                //Captura o arquivo .txt 
                StreamReader objReaderc = new StreamReader("c:\\data\\app_schemas_oracle.txt");
                //Diretório C:/
                while (sLine != null)
                {
                    sLine = objReaderc.ReadLine();
                    if (sLine != null)
                        linhasTxt.Add(sLine);
                }
                objReaderc.Close();


            }

            return linhasTxt;

        }


        //Captura parâmetros para conexão    
        public string Abrir(string chave)
        {

            ArrayList linhasTxt = new ArrayList();
            ArrayList user = new ArrayList(); // Schema do banco de dados
            ArrayList pass = new ArrayList(); // Senha do schema do banco de dados 
            ArrayList banco = new ArrayList(); // Database do banco de dados

            //ArrayList retorno = new ArrayList();
            string retorno = "";


            //Linhas do txt
            linhasTxt = LerTxt();
            int qtdList = linhasTxt.Count;

            string linha = "";
            string loc0 = "";
            string loc1 = "";
            string loc2 = "";
            string str1 = "";
            string str2 = "";
            string strChave = "";
            string passConf = "";
            string userConf = "";
            string bancoConf = "";

            //força lowercase na chave
            chave = chave.ToLower();

            //Verifica todo txt
            for (int i = 0; i <= (qtdList - 1); i++)
            {
                #region
                try
                {
                    linha = linhasTxt[i].ToString();
                    loc0 = linha.IndexOf("=").ToString();
                    strChave = linha.Substring(0, Convert.ToInt32(loc0));

                    loc1 = linha.IndexOf("/").ToString();
                    str1 = linha.Substring(Convert.ToInt32(loc0));

                    //Usuario do txt
                    user.Add(str1.Substring(1, str1.IndexOf("/") - 1));


                    str2 = linha.Substring((Convert.ToInt32(loc1) + 1));
                    loc2 = str2.IndexOf("/").ToString();

                    //Banco do txt
                    banco.Add(str2.Substring(Convert.ToInt32(loc2) + 1));

                    //Senha do txt
                    pass.Add(str2.Substring(0, Convert.ToInt32(loc2)));

                }
                catch 
                {
                    string msg = "Chave de conexão com Banco inválida.";
                    return msg;

                }
                #endregion

                if ((strChave == chave))
                {
                    userConf = user[i].ToString();
                    bancoConf = banco[i].ToString();
                    passConf = pass[i].ToString();
                    break;
                }

            }

            if (passConf != "")
            {
                //*** comentado para rodar em desenvolvimento
                CriptografiaClass objCripto = new CriptografiaClass();

                retorno = "Data Source =" + bancoConf + ";User Id =" + userConf + ";Password = " + objCripto.Decripta(passConf) + ";Integrated Security = no;";
                //retorno.Add("Data Source =" + bancoConf + ";User Id =" + userConf + ";Password = " + passConf + ";Integrated Security = no;");

            }
            else
            {
                //retorno.Add("Chave não gerada com sucesso.");
                retorno = "Chave inexistente.";

            }

            return retorno;

        }
    }
}
