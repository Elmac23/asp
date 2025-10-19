using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TaskSubmission
{
    public partial class start : Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            // dynamiczne kontrolki TextBoxP1..P10 muszą istnieć w drzewie kontroli
            // ponieważ w aspx zastosowano inline for, ASP.NET utworzy je automatycznie.
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Można wstawić domyślne wartości, np. datę dzisiejszą
                TextBoxDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }
        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            LabelError.Text = string.Empty;

            // Walidacja pól tekstowych
            string firstName = TextBoxFirstName.Text.Trim();
            string lastName = TextBoxLastName.Text.Trim();
            string dateRaw = TextBoxDate.Text.Trim();
            string course = TextBoxCourse.Text.Trim();
            string setNumberRaw = TextBoxSetNumber.Text.Trim();

            DateTime date; int setNumber = 0;
            var errors = new List<string>();

            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
            {
                errors.Add("Należy podać przynajmniej imię lub nazwisko.");
            }

            if (!string.IsNullOrEmpty(dateRaw) && !DateTime.TryParseExact(dateRaw, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                errors.Add("Data musi mieć format YYYY-MM-DD lub pozostać pusta.");
            }

            if (!string.IsNullOrEmpty(setNumberRaw) && !int.TryParse(setNumberRaw, out setNumber))
            {
                errors.Add("Numer zestawu musi być liczbą całkowitą.");
            }

            // odczyt pól z punktami
            int[] points = new int[10];
            for (int i = 1; i <= 10; i++)
            {
                var ctrl = FindControlRecursive(this, "TextBoxP" + i) as TextBox;
                if (ctrl == null) continue; // nie powinno się zdarzyć

                string txt = ctrl.Text.Trim();
                if (string.IsNullOrEmpty(txt))
                {
                    points[i - 1] = 0; // brak wpisu = 0
                    continue;
                }

                if (!int.TryParse(txt, out int p) || p < 0)
                {
                    errors.Add($"Punkty w zadaniu {i} muszą być liczbą całkowitą >= 0.");
                }
                else
                {
                    points[i - 1] = p;
                }
            }

            if (errors.Count > 0)
            {
                // walidacja nieudana — pozostawiamy formularz z zachowanymi danymi
                LabelError.Text = string.Join("<br/>", errors);
                return; // nie przekierowujemy
            }

            // walidacja OK — zapisujemy dane do Session i redirect do print.aspx
            var model = new SubmissionModel
            {
                FirstName = firstName,
                LastName = lastName,
                Date = string.IsNullOrEmpty(dateRaw) ? string.Empty : dateRaw,
                Course = course,
                SetNumber = setNumber,
                Points = points
            };

            Session["Submission"] = model;

            // Redirect (zmienia URL, powoduje GET na print.aspx)
            Response.Redirect("print.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
        protected void ButtonClear_Click(object sender, EventArgs e)
        {
            // wyczyść pola
            TextBoxFirstName.Text = string.Empty;
            TextBoxLastName.Text = string.Empty;
            TextBoxDate.Text = string.Empty;
            TextBoxCourse.Text = string.Empty;
            TextBoxSetNumber.Text = string.Empty;
            for (int i = 1; i <= 10; i++)
            {
                var ctrl = FindControlRecursive(this, "TextBoxP" + i) as TextBox;
                if (ctrl != null) ctrl.Text = string.Empty;
            }
            LabelError.Text = string.Empty;
        }

        // pomocnicza metoda do znajdowania kontrolki po ID w drzewie
        private Control FindControlRecursive(Control root, string id)
        {
            if (root == null) return null;
            var c = root.FindControl(id);
            if (c != null) return c;
            foreach (Control child in root.Controls)
            {
                var found = FindControlRecursive(child, id);
                if (found != null) return found;
            }
            return null;
        }
    }

    [Serializable]
    public class SubmissionModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Date { get; set; }
        public string Course { get; set; }
        public int SetNumber { get; set; }
        public int[] Points { get; set; }
    }
}


