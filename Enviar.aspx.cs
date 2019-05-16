using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using Microsoft.ServiceBus.Messaging;
using System.Text;
using Newtonsoft.Json;
using LinqToExcel;
using OfficeOpenXml;
using System.IO;
using System.Data;

namespace DashBoard
{
    public partial class Enviar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected class Paciente
            {
            String Nombre;
            String Departamento;
            String FechaNac;
            String EstadoNI;
            String EstadoNA;

            public string Nombre1 { get => Nombre; set => Nombre = value; }
            public string Departamento1 { get => Departamento; set => Departamento = value; }
            public string FechaNac1 { get => FechaNac; set => FechaNac = value; }
            public string EstadoNI1 { get => EstadoNI; set => EstadoNI = value; }
            public string EstadoNA1 { get => EstadoNA; set => EstadoNA = value; }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {


            if (FileUpload1.HasFile && Path.GetExtension(FileUpload1.FileName) == ".xlsx")
            {
                using (var excel = new ExcelPackage(FileUpload1.PostedFile.InputStream))
                {
                    var tbl = new DataTable();
                    var ws = excel.Workbook.Worksheets.First();
                    var hasHeader = true;  // adjust accordingly
                    foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                    {
                        tbl.Columns.Add(hasHeader ? firstRowCell.Text : String.Format("Column {0}", firstRowCell.Start.Column));
                    }

                    int startRow = hasHeader ? 2 : 1;
                    for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                    {
                        var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                        DataRow row = tbl.NewRow();
                        foreach (var cell in wsRow)
                            row[cell.Start.Column - 1] = cell.Text;
                        tbl.Rows.Add(row);
                    }



                    foreach (DataRow row in tbl.Rows)
                    {
                        Paciente paciente = new Paciente();
                        paciente.Nombre1 = row[1].ToString();
                        paciente.Departamento1 = row[2].ToString();
                        paciente.EstadoNI1 = row[3].ToString();
                        paciente.EstadoNA1 = row[4].ToString();

                        var eventHubClient = EventHubClient.CreateFromConnectionString("Endpoint=sb://dashboardumg.servicebus.windows.net/;SharedAccessKeyName=CloudBasico;SharedAccessKey=GLr7AngnFNKOXCYAAmtZmdMcDBmlRwEb5cw7WSfdJwI=", "dashboardis");
                        try
                        {
                            var message = JsonConvert.SerializeObject(paciente);
                            eventHubClient.Send(new EventData(Encoding.UTF8.GetBytes(message)));
                        }
                        catch (Exception exception)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine( exception.Message);
                            Console.ResetColor();
                        }
                    }
                }
            }



        }



        private List<Paciente> ExtraerExcel(string DireccionArchivo)
        {
            List<Paciente> listaEmpleados = new List<Paciente>();
            var AbrirExcel = new ExcelQueryFactory(DireccionArchivo);
            var ObtencionDatos = AbrirExcel.Worksheet("MARZO");

            foreach (var elementos in ObtencionDatos)
            {
                var nuevoEmpleado = new Paciente
                {
                    Nombre1 = elementos.ElementAt(0).Value.ToString(),
                    Departamento1 = elementos.ElementAt(1).Value.ToString(),
                    EstadoNI1 = elementos.ElementAt(2).ToString(),
                    EstadoNA1 = elementos.ElementAt(3).ToString(),
                };

                listaEmpleados.Add(nuevoEmpleado);
            }

            return listaEmpleados;
        }
    }
}