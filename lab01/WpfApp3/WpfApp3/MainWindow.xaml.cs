using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.cb_pizzaType.Items.Add("4sajtos");
            this.cb_pizzaType.Items.Add("songoku");
            this.cb_pizzaType.Items.Add("magyaros");
            this.cb_pizzaType.Items.Add("szalámis");
            this.cb_pizzaType.Items.Add("mindenmentes");
            this.cb_pizzaType.Items.Add("húsimádó");
            this.cb_pizzaType.Items.Add("hawaii");

            this.cb_pizzaType.SelectedIndex = 0;
            this.rb_tomato.IsChecked = true;

            if (File.Exists("pizza.json"))
            {
                var pizzas = JsonConvert
                    .DeserializeObject<List<Pizza>>(File.ReadAllText("pizza.json"));
                pizzas.ForEach(p => lbox_orders.Items.Add(p));
            }
        }

        private void OrderClick(object sender, RoutedEventArgs e)
        {
            PizzaBase pizzaBase = rb_tomato.IsChecked == true ?
                PizzaBase.tomato : PizzaBase.sourCream;
            List<string> extras = new List<string>();
            foreach (var item in wp_extras.Children)
            {
                //if (item is CheckBox)
                //{
                //    CheckBox cb = (CheckBox)item;
                //}

                if (item is CheckBox cb && cb.IsChecked == true)
                    extras.Add(cb.Content.ToString());
            }

            string pizzaType = cb_pizzaType.SelectedItem.ToString();
            string customerName = tbox_customer_name.Text;

            Pizza p = new Pizza(customerName, pizzaType, pizzaBase, extras);

            lbox_orders.Items.Add(p);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            List<Pizza> pizzas = new List<Pizza>();
            foreach (var item in lbox_orders.Items)
            {
                pizzas.Add(item as Pizza);
            }

            string jsonData = JsonConvert.SerializeObject(pizzas);
            File.WriteAllText("pizza.json", jsonData);
        }
    }

    enum PizzaBase
    {
        tomato,
        sourCream,
    }

    class Pizza
    {
        public string CustomerName { get; private set; }
        public string PizzaType { get; private set; }
        public PizzaBase PizzaBase { get; private set; }
        public List<string> Extras { get; private set; }

        public Pizza(string customerName, string pizzaType, PizzaBase pizzaBase, List<string> extras)
        {
            CustomerName = customerName;
            PizzaType = pizzaType;
            PizzaBase = pizzaBase;
            Extras = extras;
        }

        public override string ToString()
        {
            return $"({CustomerName}) {PizzaType}, {PizzaBase}, extrák: " +
                $"{String.Join(", ", Extras)}";
        }
    }
}