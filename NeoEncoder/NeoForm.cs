using NeoEncoder.Neo_coder;

namespace NeoEncoder
{
    public partial class NeoForm : Form
    {
        public NeoForm()
        {
            InitializeComponent();
            coder = new Coder();
            coder.E_infoSize = Info;
        }

        Coder coder;

        private void button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog OPF = new OpenFileDialog())
            {
                OPF.FileName = textBox1.Text;
                if (OPF.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = OPF.FileName;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!File.Exists(textBox1.Text) || comboBox1.SelectedIndex < 0) { return; }
            textBox1.Enabled = false;
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    coder.DecodeRLEBWT(File.ReadAllBytes(textBox1.Text), textBox1.Text);
                    break;
                case 1:
                    coder.DecodeLZW(File.ReadAllBytes(textBox1.Text), textBox1.Text);
                    break;
                case 2:
                    coder.DecodeLZWBWT(File.ReadAllBytes(textBox1.Text), textBox1.Text);
                    break;
                default: break;
            }
            textBox1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!File.Exists(textBox1.Text) || comboBox1.SelectedIndex < 0) { return; }
            textBox1.Enabled = false;
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    coder.EncodeRLEBWT(File.ReadAllBytes(textBox1.Text), textBox1.Text);
                    break;
                case 1:
                    coder.EncodeLZW(File.ReadAllBytes(textBox1.Text), textBox1.Text);
                    break;
                case 2:
                    coder.EncodeLZWBWT(File.ReadAllBytes(textBox1.Text), textBox1.Text);
                    break;
                default: break;
            }
            textBox1.Enabled = true;
        }
        private void Info(int sB, int sE)
        {
            MessageBox.Show(
                 $"�������� ������: {sB} ����\n�������� ������: {sE} ����\n����������� ������: {Math.Round(1 - sE / (double)sB, 2)}",
                "Info");
        }
    }
}
