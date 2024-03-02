using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySqlConnector;
using AppMySQL.Models;

namespace AppMySQL.Controller
{
    public class MySQLCon
    {
        static string conn = @"server=sql.freedb.tech;port=3306;database=freedb.MucaLopesDB_TDS10;user=freedb_MucaLopes;password=$wm7!EMhj6xRxZ5";

        public static List<Pessoa> ListarPessoas()
        {
            List<Pessoa> listapessoas = new List<Pessoa>();
            string sql = "SELECT * FROM pessoa";
            using (MySqlConnection con = new MySqlConnection(conn))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Pessoa pessoa = new Pessoa()
                            {
                                ID = reader.GetInt32(0),
                                Nome = reader.GetString(1),
                                Idade = reader.GetString(2),
                                Cidade = reader.GetString(3),
                            };
                            listapessoas.Add(pessoa);
                        }
                    }
                }
                con.Close();
                return listapessoas;
            }
        }
        //parametros algo mais direto, usado somente para poucos dados, mais recomendável procedure para grandes valores.
        public static void InserirPessoa(string nome, string idade, string cidade) //parametros, variáveis que recebem os valores
        {
            string sql = "INSERT INTO pessoa(Nome,Idade,Cidade) VALUES (@Nome,@Idade,@Cidade)";
            using (MySqlConnection con = new MySqlConnection(conn))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = nome;
                    cmd.Parameters.Add("@idade", MySqlDbType.VarChar).Value = idade;
                    cmd.Parameters.Add("@cidade", MySqlDbType.VarChar).Value = cidade;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
        }
        //procedure resume e evita criação de muitos parametros por meio do Model
        public void AtualizarPessoa(Pessoa pessoa) //procedure, puxa os dados Model Pessoa através de "pessoa" para receber os valores
        {
            string sql = "UPDATE pessoa SET nome=@nome,idade=@idade,cidade=@cidade WHERE id=@id";
            try
            {
                using (MySqlConnection con = new MySqlConnection(conn))
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = pessoa.Nome;
                        cmd.Parameters.Add("@idade", MySqlDbType.VarChar).Value = pessoa.Idade;
                        cmd.Parameters.Add("@cidade", MySqlDbType.VarChar).Value = pessoa.Cidade;
                        cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = pessoa.ID;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
            }
            catch (Exception )
            {

            }
        }

        public static void ExcluirPessoa(Pessoa pessoa)
        {
            string sql = "DELETE FROM pessoa WHERE id=@id";
            using (MySqlConnection con = new MySqlConnection(conn))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = pessoa.ID;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
        }
    }
}