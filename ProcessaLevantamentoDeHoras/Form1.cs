using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ProcessaLevantamentoDeHoras
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                RestoreDirectory = true,
                Title = "Localizar arquivo",
                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 2,
                CheckFileExists = true,
                CheckPathExists = true,
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            label2.Text = ofd.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var text = File.ReadAllText(label2.Text);

            var source = text.Split(new[] { '.', '?', '!', ' ', ';', ':', ',' },
                StringSplitOptions.RemoveEmptyEntries);

            var totais = new Dictionary<string, int>
            {
                { Item.Script.Short(Complexidade.Simples), source.Count(x => x == "01") },
                { Item.Api.Short(Complexidade.Simples), source.Count(x => x == "02") },
                { Item.Formulario.Short(Complexidade.Simples), source.Count(x => x == "03") },
                { Item.Tabela.Short(Complexidade.Simples), source.Count(x => x == "04") },
                { Item.Pdf.Short(Complexidade.Simples), source.Count(x => x == "05") },
                { Item.Excel.Short(Complexidade.Simples), source.Count(x => x == "06") },
                { Item.Job.Short(Complexidade.Simples), source.Count(x => x == "07") },

                { Item.Script.Short(Complexidade.Media), source.Count(x => x == "11") },
                { Item.Api.Short(Complexidade.Media), source.Count(x => x == "12") },
                { Item.Formulario.Short(Complexidade.Media), source.Count(x => x == "13") },
                { Item.Tabela.Short(Complexidade.Media), source.Count(x => x == "14") },
                { Item.Pdf.Short(Complexidade.Media), source.Count(x => x == "15") },
                { Item.Excel.Short(Complexidade.Media), source.Count(x => x == "16") },
                { Item.Job.Short(Complexidade.Media), source.Count(x => x == "17") },

                { Item.Script.Short(Complexidade.Complexa), source.Count(x => x == "21") },
                { Item.Api.Short(Complexidade.Complexa), source.Count(x => x == "22") },
                { Item.Formulario.Short(Complexidade.Complexa), source.Count(x => x == "23") },
                { Item.Tabela.Short(Complexidade.Complexa), source.Count(x => x == "24") },
                { Item.Pdf.Short(Complexidade.Complexa), source.Count(x => x == "25") },
                { Item.Excel.Short(Complexidade.Complexa), source.Count(x => x == "26") },
                { Item.Job.Short(Complexidade.Complexa), source.Count(x => x == "27") },

            };

            var inicio = text.IndexOf("#!", StringComparison.Ordinal);
            text = text.Remove(inicio, (text.IndexOf("!#", StringComparison.Ordinal) - inicio) + 2);

            text = text
                .Replace("01:", Item.Script.Build(Complexidade.Simples))
                .Replace("02:", Item.Api.Build(Complexidade.Simples))
                .Replace("03:", Item.Formulario.Build(Complexidade.Simples))
                .Replace("04:", Item.Tabela.Build(Complexidade.Simples))
                .Replace("05:", Item.Pdf.Build(Complexidade.Simples))
                .Replace("06:", Item.Excel.Build(Complexidade.Simples))
                .Replace("07:", Item.Job.Build(Complexidade.Simples))

                .Replace("11:", Item.Script.Build(Complexidade.Media))
                .Replace("12:", Item.Api.Build(Complexidade.Media))
                .Replace("13:", Item.Formulario.Build(Complexidade.Media))
                .Replace("14:", Item.Tabela.Build(Complexidade.Media))
                .Replace("15:", Item.Pdf.Build(Complexidade.Media))
                .Replace("16:", Item.Excel.Build(Complexidade.Media))
                .Replace("17:", Item.Job.Build(Complexidade.Media))

                .Replace("21:", Item.Script.Build(Complexidade.Complexa))
                .Replace("22:", Item.Api.Build(Complexidade.Complexa))
                .Replace("23:", Item.Formulario.Build(Complexidade.Complexa))
                .Replace("24:", Item.Tabela.Build(Complexidade.Complexa))
                .Replace("25:", Item.Pdf.Build(Complexidade.Complexa))
                .Replace("26:", Item.Excel.Build(Complexidade.Complexa))
                .Replace("27:", Item.Job.Build(Complexidade.Complexa));

            text += "\n";
            text = totais.Where(x => x.Value > 0).Aggregate(text, (current, total) => current + $"\n {total.Value} {total.Key}");

            File.WriteAllText($"{label2.Text.Replace(".txt", "")} (processed).txt", text);

            MessageBox.Show("Arquivo processado com sucesso na mesma pasta da arquivo original");
        }
    }

    public enum Item
    {
        Script = 1,
        Api = 2,
        Formulario = 3,
        Tabela = 4,
        Pdf = 5,
        Excel = 6,
        Job = 7,
    }

    public enum Complexidade
    {
        Simples = 0,
        Media = 1,
        Complexa = 2,
    }

    public static class BuildItem
    {
        public static string ToText(this Item item)
        {
            switch ((byte)item)
            {
                case 1: return "Script";
                case 2: return "Api";
                case 3: return "Formulário";
                case 4: return "Tabela";
                case 5: return "Pdf";
                case 6: return "Excel";
                case 7: return "Job";
                default: return "Item não identificado";
            }
        }

        public static string ToText(this Complexidade complexidade)
        {
            switch ((byte)complexidade)
            {
                case 0: return "simples";
                case 1: return "média";
                case 2: return "complexa";
                default: return "complexidade não identificada";
            }
        }

        public static string Build(this Item item, Complexidade complexidade)
            => $"Alteração/criação de {item.ToText()} de complexidade {complexidade.ToText()};";

        public static string Short(this Item item, Complexidade complexidade)
            => $"{item.ToText()} {complexidade.ToText()}";
    }
}
