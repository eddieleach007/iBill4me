using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Xml.Serialization;
using System.Xml;

namespace iBill4mev1
{
    public partial class Collection_Report : Form
    {
        //Start - my Variables.


        //item Count.
        int PCLCount = 0;
        int PC5Count = 0;
        int TFTCount = 0;
        int LPCount = 0;
        int PCSCount = 0;
        int OVCount = 0;
        int OTCount = 0;
        int datawipeCount = 0;
        int datawipePCL = 0;
        int datawipePC5 = 0;
        int datawipePCS = 0;
        int datawipeOT = 0;
        int addHDDCount = 0;
        int itemtotal = 0;

        //total results
        double pclTotal = 0;
        double pc5Total = 0;
        double lpTotal = 0;
        double tftTotal = 0;
        double pcsTotal = 0;
        double otTotal = 0;
        double addHDDTotal = 0;
        double datawipeTotal = 0;

        public string myDate;

        int tableSize; //used in DVLAS

        DateTime startbillingdate = new DateTime(2015, 03, 24); //used in DVLAS

        //End - my Variables.

        public Collection_Report()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'iBill4meDataSet.DVLAC' table. You can move, or remove it, as needed.
            this.dVLACTableAdapter1.Fill(this.iBill4meDataSet.DVLAC);
        }
        
        //Start of CollectionReportButton
        private void CollectionReportButton_Click(object sender, EventArgs e)
        {
            resetAll();
            this.iBill4meDataSet.Clear();
            //this.dVLACTableAdapter1.Fill(this.iBill4meDataSet.DVLAC);
            this.dataGridView.DataSource = this.iBill4meDataSet.DVLAC;
            
            SqlConnection c = new SqlConnection(Properties.Settings.Default.iBill4meConnectionString);
            c.Open();
            //SqlDataAdapter a = new SqlDataAdapter("SELECT Barcode, [Item Type]  FROM myCollectionReport", c);
            //SqlDataAdapter a = new SqlDataAdapter("SELECT * FROM DVLA_COL", c);
            SqlDataAdapter a = new SqlDataAdapter("SELECT * FROM DVLAC", c);
            DataTable dt1 = this.iBill4meDataSet.DVLAC;
            a.Fill(dt1);
            c.Close();
            /////////////////////////////
            myHeaderLabel.Text = "DVLA COLLECTION REPORT";
            /////////////////////////////

            string[] barcodeArray = new string[dt1.Rows.Count];
            string[] collectionnoArray = new string[dt1.Rows.Count];
            string[] manufacterArray = new string[dt1.Rows.Count];
            string[] itemtypeArray = new string[dt1.Rows.Count];
            string[] modelArray = new string[dt1.Rows.Count];
            string[] descriptionArray = new string[dt1.Rows.Count];
            string[] serialnoArray = new string[dt1.Rows.Count];
            string[] assetArray = new string[dt1.Rows.Count];
            string[] asset2Array = new string[dt1.Rows.Count];
            string[] asset3Array = new string[dt1.Rows.Count];
            string[] asset4Array = new string[dt1.Rows.Count];
            string[] extrabarcodeArray = new string[dt1.Rows.Count];
            string[] speedArray = new string[dt1.Rows.Count];
            string[] memoryArray = new string[dt1.Rows.Count];
            string[] diskcapacityArray = new string[dt1.Rows.Count];
            string[] tagArray = new string[dt1.Rows.Count];
            string[] gradeArray = new string[dt1.Rows.Count];
            string[] specArray = new string[dt1.Rows.Count];
            string[] faultsArray = new string[dt1.Rows.Count];

            string col1 = "Barcode";
            string col2 = "Collection No#";
            string col3 = "Manufacturer";
            string col4 = "Item Type";
            string col5 = "Model";
            string col6 = "Description";
            string col7 = "Serial No";
            string col8 = "Asset No";
            string col9 = "Asset No2";
            string col10 = "Asset No3";
            string col11 = "Asset No4";
            string col12 = "Extra Barcode";
            string col13 = "Speed";
            string col14 = "Memory";
            string col15 = "Disk Capacity";
            string col16 = "Tag";
            string col17 = "Grade";
            string col18 = "Specification";
            string col19 = "Faults";

            for (int index = 0; index < dt1.Rows.Count; index++)
            {
                barcodeArray[index] = dt1.Rows[index][col1].ToString();
                collectionnoArray[index] = dt1.Rows[index][col2].ToString();
                manufacterArray[index] = dt1.Rows[index][col3].ToString();
                itemtypeArray[index] = dt1.Rows[index][col4].ToString();
                modelArray[index] = dt1.Rows[index][col5].ToString();
                descriptionArray[index] = dt1.Rows[index][col6].ToString();
                serialnoArray[index] = dt1.Rows[index][col7].ToString();
                assetArray[index] = dt1.Rows[index][col8].ToString();
                asset2Array[index] = dt1.Rows[index][col9].ToString();
                asset3Array[index] = dt1.Rows[index][col10].ToString();
                asset4Array[index] = dt1.Rows[index][col11].ToString();
                extrabarcodeArray[index] = dt1.Rows[index][col12].ToString();
                speedArray[index] = dt1.Rows[index][col13].ToString();
                memoryArray[index] = dt1.Rows[index][col14].ToString();
                diskcapacityArray[index] = dt1.Rows[index][col15].ToString();
                tagArray[index] = dt1.Rows[index][col16].ToString();
                gradeArray[index] = dt1.Rows[index][col17].ToString();
                specArray[index] = dt1.Rows[index][col18].ToString();
                faultsArray[index] = dt1.Rows[index][col19].ToString();
                string[] OTitemArray = new string[] { "SWI", "OT", "OD", "CAB", "SCA", "FAX", "HDD", "DOC" };
                string[] OVitemArray = new string[] { "OV", "PAL"};

                //OV and needs it's own label and settings.
                //HDD have also been included in but are including parent barcodes at the moment.
                //
                if (itemtypeArray[index] == "PCL")
                {
                    PCLCount++;
                }
                else if (itemtypeArray[index] == "PC5")
                {
                    PC5Count++;
                }
                else if ((itemtypeArray[index] == "TFT") || (itemtypeArray[index] == "PCM") || (itemtypeArray[index] == "TOU"))
                {
                    TFTCount++;
                }
                else if ((itemtypeArray[index] == "LP") || (itemtypeArray[index] == "MFP"))
                {
                    LPCount++;
                }
                else if (itemtypeArray[index] == "PCS")
                {
                    PCSCount++;
                }
                else if (OTitemArray.Contains(itemtypeArray[index].ToString()))
                {
                    OTCount++;
                }
                else if (OVitemArray.Contains(itemtypeArray[index].ToString()))
                {
                    OVCount++;
                }

                //Datawipe
                string myBlancco = "Blancco";
                string myHDD = "HDD REMOVED";
                //Look for "No Parent System
                //Extra Barcode.
                //Extra Asset Number Field.


                if(faultsArray[index].ToString().Contains(myBlancco))
                {
                    //See if an item has been data wiped.
                    if (itemtypeArray[index] == "PCL")
                    {
                        datawipePCL++;
                    }
                    else if (itemtypeArray[index] == "PC5")
                    {
                        datawipePC5++;
                    }
                    else if (itemtypeArray[index] == "PCS")
                    {
                        datawipePCS++;
                    }

                    if (itemtypeArray[index] == "HDD")
                    {
                        if ((assetArray[index] == "NO PARENT SYSTEM") || (extrabarcodeArray[index] == "NO PARENT SYS"))
                        {
                            addHDDCount++;
                        }

                    }
                    datawipeCount = datawipePCL + datawipePC5 + datawipePCS + addHDDCount + datawipeOT;
                    
                }
                else if(faultsArray[index].ToString().Contains(myHDD))
                {
                    if (itemtypeArray[index] == "PCL")
                    {
                        datawipePCL++;
                    }
                    else if (itemtypeArray[index] == "PC5")
                    {
                        datawipePC5++;
                    }
                    else if (itemtypeArray[index] == "PCS")
                    {
                        datawipePCS++;
                    
                    }

                    //Checks to see if it's an extra barcode. 
                    if (itemtypeArray[index] == "HDD")
                    {
                        if((assetArray[index] == "NO PARENT SYSTEM") || (extrabarcodeArray[index] == "NO PARENT SYS"))
                        {
                            addHDDCount++;
                        }
                        
                    }
                    
                    datawipeCount = datawipePCL + datawipePC5 + datawipePCS + datawipeOT + addHDDCount;
                   //MessageBox.Show(barcodeArray[index].ToString() + itemtypeArray[index].ToString() + faultsArray[index].ToString());
                }
            }
            //MessageBox.Show(addHDDCount.ToString());
            //Start - Calculating Results
            pclCountLabel.Text = Convert.ToString(PCLCount);
            pc5CountLabel.Text = Convert.ToString(PC5Count);
            lpCountLabel.Text = Convert.ToString(LPCount);
            tftLabelCount.Text = Convert.ToString(TFTCount);
            pcsCountLabel.Text = Convert.ToString(PCSCount);
            ovsizeCountLabel.Text = Convert.ToString(OVCount);
            OTCountLabel.Text = Convert.ToString(OTCount);
            datawipeCountLabel.Text = Convert.ToString(datawipeCount);
            addHDDTotal = addHDDCount;
            datawipePCLLabel.Text = Convert.ToString(datawipePCL);
            datawipePC5Label.Text = Convert.ToString(datawipePC5);
            datawipePCSLabel.Text = Convert.ToString(datawipePCS);
            datawipeHDDLabel.Text = Convert.ToString(addHDDTotal);
            datawipeOTLabel.Text = Convert.ToString(datawipeOT);
            //74 HDD Datawipe.
            //181 Datawipe Total.

            
            datawipeTotal = datawipeCount;
            itemtotal = PCLCount + PC5Count + LPCount + TFTCount + PCSCount + OTCount  + OVCount;
            int mytableRowCount = dt1.Rows.Count;
            rowCountTotalLabel.Text = Convert.ToString(mytableRowCount);
            itemCountTotal.Text = Convert.ToString(itemtotal) + " not including data wipe items.";
            //End - Calculating Results

            //Checks to see if everything has been billed.
            if (mytableRowCount != itemtotal)
            {
                MessageBox.Show("Figures don't match!");
            }
            else
            {
                MessageBox.Show("Everything has been billed!");
            }
        }
        //end of CollectionReportButton


        //Start of StockReportButton
        private void StockReportButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            Stock_Report sr = new Stock_Report();
            sr.Show();

        }
        //EndofStockReportButton


        //Start of StockReportButton
        private void DispatchReportButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            Dispatch_Report dr = new Dispatch_Report();
            dr.Show();

        }
        //EndofStockReportButton

        //Start of resetAll
        void resetAll()
        {
            PCLCount = 0;
            pclTotal = 0;
            PC5Count = 0;
            pc5Total = 0;
            LPCount = 0;
            lpTotal = 0;
            TFTCount = 0;
            tftTotal = 0;
            PCSCount = 0;
            pcsTotal = 0;
            OTCount = 0;
            otTotal = 0;
            datawipeCount = 0;
            datawipeTotal = 0;
            itemtotal = 0;
        }
        //end of resetAll


        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
