using System;
using dio_dotnet_cadastro.Classes;
using dio_dotnet_cadastro.Enums;

namespace dio_dotnet_cadastro
{
    class Program
    {
        static SerieRepositorio repositorio = new SerieRepositorio();

        static void Main(string[] args)
        {
            string opcaoUsuario = ObterOpcaoUsuario();

            while (opcaoUsuario.ToUpper() != "X")
            {
                switch (opcaoUsuario)
                {
                    case "1":
                        ListarSeries();
                        break;

                    case "2":
                        InserirSerie();
                        break;

                    case "3":
                        AtualizarSerie();
                        break;

                    case "4":
                        ExcluirSerie();
                        break;

                    case "5":
                        VisualizarSerie();
                        break;

                    case "C":
                        Console.Clear();
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                opcaoUsuario = ObterOpcaoUsuario();
            }
        }

        public static string ObterOpcaoUsuario()
        {
            Console.WriteLine();
            Console.WriteLine("Informe a opção desejada:");
            Console.WriteLine();
            Console.WriteLine("1 - Listar séries");
            Console.WriteLine("2 - Inserir nova série");
            Console.WriteLine("3 - Atualizar série");
            Console.WriteLine("4 - Excluir série");
            Console.WriteLine("5 - Visualizar série");
            Console.WriteLine("C - Limpar tela");
            Console.WriteLine("X - Sair");
            Console.WriteLine();

            string opcaoUsuario = Console.ReadLine().ToUpper();
            Console.WriteLine();

            return opcaoUsuario;

        }

        public static void ListarSeries()
        {
            Console.WriteLine("Listar séries...");
            
            var lista = repositorio.Lista();

            if (lista.Count == 0)
            {
                Console.WriteLine("Nenhuma série encontrada...");
                return;
            }

            foreach (var serie in lista)
            {
                var excluido = serie.retornaExcluido();
                
                if (!excluido)
                {
                    Console.WriteLine("#ID {0}: - {1}", serie.retornaId(), serie.retornaTitulo());
                }
            }
        }

        private static void retornaInformacoes(out int entradaGenero, 
                                            out string entradaTitulo,
                                            out int entradaAno,
                                            out string entradaDescricao)
        {
            foreach (int i in Enum.GetValues(typeof(Genero)))
            {
                Console.WriteLine("{0} - {1}", i, Enum.GetName(typeof(Genero), i));
            }

            Console.WriteLine("Escolha o gênero dentre as opções acima: ");
            entradaGenero = int.Parse(Console.ReadLine());

            Console.WriteLine("Insira o título da série: ");
            entradaTitulo = Console.ReadLine();

            Console.WriteLine("Insira o ano de início da série: ");
            entradaAno = int.Parse(Console.ReadLine());

            Console.WriteLine("Insira a descrição da série: ");
            entradaDescricao = Console.ReadLine();
        }

        private static bool checaExiste(int id)
        {
            if (id <= repositorio.ProximoId() || !repositorio.RetornaPorId(id).retornaExcluido())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void InserirSerie()
        {
            Console.WriteLine("Inserir nova série:");

            int entradaGenero;
            string entradaTitulo;
            int entradaAno;
            string entradaDescricao;
            retornaInformacoes(out entradaGenero, out entradaTitulo, out entradaAno, out entradaDescricao);

            Serie novaSerie = new Serie(id: repositorio.ProximoId(),
                                        genero: (Genero)entradaGenero,
                                        titulo: entradaTitulo,
                                        ano: entradaAno,
                                        descricao: entradaDescricao);

            repositorio.Insere(novaSerie);
        }

        private static void AtualizarSerie()
        {
            Console.WriteLine("Insira o ID da série que deseja atualizar: ");
            int indiceSerie = int.Parse(Console.ReadLine());

            if (!checaExiste(indiceSerie))
            {
                Console.WriteLine("Série não encontrada...");
                return;
            } 
            else 
            {
                int entradaGenero;
                string entradaTitulo;
                int entradaAno;
                string entradaDescricao;
                retornaInformacoes(out entradaGenero, out entradaTitulo, out entradaAno, out entradaDescricao);

                Serie atualizaSerie = new Serie(id: indiceSerie,
                                            genero: (Genero)entradaGenero,
                                            titulo: entradaTitulo,
                                            ano: entradaAno,
                                            descricao: entradaDescricao);

                repositorio.Atualiza(indiceSerie, atualizaSerie);
            }
        }

        private static void ExcluirSerie()
        {
            Console.WriteLine("Insira o ID da série que deseja excluir: ");
            int indiceSerie = int.Parse(Console.ReadLine());

            if (!checaExiste(indiceSerie))
            {
                Console.WriteLine("Série não encontrada...");
                return;
            } 
            else 
            {
                Console.WriteLine("Você realmente deseja excluir essa série? S/N");
                if (Console.ReadLine().ToUpper() == "S")
                {
                    repositorio.Exclui(indiceSerie);
                } 
                else
                {
                    return;
                }
            }
        }

        public static void VisualizarSerie()
        {
            Console.WriteLine("Insira o ID da série que deseja visualizar: ");
            int indiceSerie = int.Parse(Console.ReadLine());

            if (!checaExiste(indiceSerie)) 
            {
                Console.WriteLine("Série não encontrada...");
                return;
            }
            else
            {
                var serie = repositorio.RetornaPorId(indiceSerie);
                Console.WriteLine(serie);
            }
        }
    }
}
