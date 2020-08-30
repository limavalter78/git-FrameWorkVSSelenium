using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudo.Framework.VSSelenium.Wrapper
{
    public class HtmlTable : HtmlControl
    {
        private string rowFilterXpath = "descendant::tr";

        public HtmlTable(IWebElement webElement) : base(webElement)
        {
        }

        public string RowFilterXpath
        {
            get { return rowFilterXpath; }
            set { rowFilterXpath = value; }
        }

        public int TopRowsToIgnore { get; set; }
        public int BottomRowsToIgnore { get; set; }

        public List<HtmlRow> Rows
        {
            get
            {
                List<HtmlRow> rows = webElement.FindElements(By.XPath(rowFilterXpath)).
                    Select(x => new HtmlRow { WebElement = x }).ToList();
                rows.RemoveRange(0, TopRowsToIgnore);
                rows.RemoveRange(rows.Count - BottomRowsToIgnore, BottomRowsToIgnore);
                return rows;
            }
        }

        public int RowCount { get; internal set; }

        public HtmlRow GetRow(int position)
        {
            return Rows[position];
        }

        public HtmlCell GetCell(int rowIndex, int columnIndex)
        {
            return Rows[rowIndex].Cells[columnIndex];
        }
    }

    public class HtmlRow
    {
        public IWebElement WebElement { get; set; }

        private string cellFilterXpath = "descendant::td";
        public int TopRowsToIgnore { get; set; }
        public int BottomRowsToIgnore { get; set; }

        public List<HtmlCell> Cells
        {
            get
            {
                List<HtmlCell> cells = WebElement.FindElements(By.XPath(cellFilterXpath)).
                    Select(x => new HtmlCell { WebElement = x }).ToList();


                cells.RemoveRange(0, TopRowsToIgnore);
                cells.RemoveRange(cells.Count - BottomRowsToIgnore, BottomRowsToIgnore);
                return cells;
            }
        }

    }

    public class HtmlCell
    {
        /// <summary>
        /// Retorna os filhos de uma cell, filtrados pela tag
        /// </summary>
        /// <param name="cssSelector">string nome da tag que deseja retornar</param>
        /// <returns>lista de IWebElement</returns>
        public ICollection<IWebElement> GetChildren(string cssSelector)
        {
            return WebElement.FindElements(By.CssSelector(cssSelector));
        }

        /// <summary>
        /// Busca todos elementos filhos da celula
        /// </summary>
        /// <returns>lista de IWebElement</returns>
        public ICollection<IWebElement> GetChildren()
        {
            return WebElement.FindElements(By.XPath("descendant::*"));
        }

        public IWebElement WebElement { get; set; }
    }
}
