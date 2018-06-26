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
    public partial class Dispatch_Report : Form
    {

        //Start of Variables
        int PCLCount = 0;
        int PC5Count = 0;
        int HDDCount = 0;
        int PCSCount = 0;
        int TFTCount = 0;
        int PrinterCount = 0;
        int OTCount = 0;
        int itemCountTotal = 0;

        int tableRowCount;

        int disposalTotal = 0;
        int soldCount = 0;
        int redeployCount = 0;
        int nonehazdisposalCount = 0;
        int hazdisposalCount = 0;
        int palletCount = 0;
         
        int screenDisposalCount = 0;
        int stdDisposalWithHDD = 0;
        int stdDisposalWithoutHDD = 0;
        int highDisposalWithHDD = 0;
        int highDisposalWithoutHDD = 0;

        
       

        double soldValueTotal = 0;

        int PatTest = 0;
        int CleanTest = 0;
        int ReboxTest = 0;

        public string[] OTitemArray = new string[] { "SWI", "OT", "OD", "CAB", "SCA", "FAX" };

        //End of Variables
        

        private void Dispatch_Report_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'iBill4meDataSet.DVLAD' table. You can move, or remove it, as needed.
            myHeaderLabel.Text = "DVLA DISPATCH REPORT";
            this.dVLADTableAdapter.Fill(this.iBill4meDataSet.DVLAD);
        }

        //Start of Dispatch Report
        private void DispatchReportButton_Click(object sender, EventArgs e)
        {
            resetAll();
            this.iBill4meDataSet.Clear();
            /////////////
            //SQL Connection to Database
            this.dataGridView.DataSource = this.iBill4meDataSet.DVLAD;
            SqlConnection c = new SqlConnection(Properties.Settings.Default.iBill4meConnectionString);
            c.Open();
            SqlDataAdapter a = new SqlDataAdapter("SELECT * FROM DVLAD", c);
            DataTable dt = this.iBill4meDataSet.DVLAD;
            a.Fill(dt);
            c.Close();
            /////////////

            /////////////
            
            string[] despatchdateArray = new string[dt.Rows.Count];
            string[] ordernoArray = new string[dt.Rows.Count];
            string[] barcodeArray = new string[dt.Rows.Count];
            string[] collectionnoArray = new string[dt.Rows.Count];
            string[] collectiondateArray = new string[dt.Rows.Count];
            string[] manufacturerArray = new string[dt.Rows.Count];
            string[] itemtypeArray = new string[dt.Rows.Count];
            string[] modelArray = new string[dt.Rows.Count];
            string[] descriptionArray = new string[dt.Rows.Count];
            string[] gradeArray = new string[dt.Rows.Count];
            string[] tagArray = new string[dt.Rows.Count];
            string[] serialNo = new string[dt.Rows.Count];
            string[] assetNo = new string[dt.Rows.Count];
            string[] clientpriceArray = new string[dt.Rows.Count];
            string[] salepriceArray = new string[dt.Rows.Count];

            string col1 = "Despatch Date";
            string col2 = "Order No#";
            string col3 = "Barcode";
            string col4 = "Collection No#";
            string col5 = "Collection Date";
            string col6 = "Manufacturer";
            string col7 = "Item Type";
            string col8 = "Model";
            string col9 = "Description";
            string col10 = "Grade";
            string col11 = "Tag";
            string col12 = "Serial No#";
            string col13 = "Asset No#";
            string col14 = "Client Price";
            string col15 = "Sale Price";

            for (int index = 0; index < dt.Rows.Count; index++)
            {
                despatchdateArray[index] = dt.Rows[index][col1].ToString();
                ordernoArray[index] = dt.Rows[index][col2].ToString();
                barcodeArray[index] = dt.Rows[index][col3].ToString();
                collectionnoArray[index] = dt.Rows[index][col4].ToString();
                collectiondateArray[index] = dt.Rows[index][col5].ToString();
                manufacturerArray[index] = dt.Rows[index][col6].ToString();
                itemtypeArray[index] = dt.Rows[index][col7].ToString();
                modelArray[index] = dt.Rows[index][col8].ToString();
                descriptionArray[index] = dt.Rows[index][col9].ToString();
                gradeArray[index] = dt.Rows[index][col10].ToString();
                tagArray[index] = dt.Rows[index][col11].ToString();
                serialNo[index] = dt.Rows[index][col12].ToString();
                assetNo[index] = dt.Rows[index][col13].ToString();
                clientpriceArray[index] = dt.Rows[index][col14].ToString();
                salepriceArray[index] = dt.Rows[index][col15].ToString();
           
                //Start of SALE items
                if((tagArray[index] == "SALE") || (tagArray[index] == "Sale"))
                {
                    soldValueTotal = soldValueTotal + Convert.ToDouble(salepriceArray[index]);
                    soldCount++;
                    int mysalepriceCheck = Convert.ToInt32(salepriceArray[index]);
                    if(mysalepriceCheck == 0)
                    {
                        MessageBox.Show("Check Price for Barcode: " + barcodeArray[index].ToString() + ". Contact team for price.");
                    }
                }
                //End of SALE items

                //Start of Redeployment
                else if((tagArray[index] == "Redeployed")  || (tagArray[index] == "REDEPLOYED"))
                {
                    if (itemtypeArray[index] == "PC5")
                    {
                        PC5Count++;
                        CleanTest++;
                        PatTest++;
                        ReboxTest++;
                        
                    }
                    else if (itemtypeArray[index] == "PCL")
                    {
                        PCLCount++;
                        CleanTest++;
                        PatTest++;
                        ReboxTest++;
                    }
                    else if (itemtypeArray[index] == "PCS")
                    {
                        CleanTest++;
                        PatTest++;
                        ReboxTest++;
                        PCSCount++;
                    }
                    else if (itemtypeArray[index] == "TFT")
                    {
                        CleanTest++;
                        PatTest++;
                        ReboxTest++;
                        TFTCount++;
                    }
                    else if ((itemtypeArray[index] == "LP") || (itemtypeArray[index] == "MFP"))
                    {
                        CleanTest++;
                        PatTest++;
                        ReboxTest++;
                        PrinterCount++;
                    }
                    else
                    {
                        OTCount++;
              
                    
                    }
                  
                }
                //End of Redeployment

                //Start of Disposal
                else if ((tagArray[index] == "Haz Standard Security") || (tagArray[index] == "Standard Security Service") || (tagArray[index] == "General Scrap Disposal") || (tagArray[index] == "Dataserve"))
                {//Start of Standard Secure and Hazadous Standard Secure Disposal
                    string[] withoutHDDArray = new string[] { "LP", "MFP", "OT", "OD", "SWI", "FAX" };
                    string[] withHDDArray = new string[] { "HDD", "PC5", "PC4", "PCS" };
                    string[] screenArray = new string[] { "PCL", "PCM", "TFT"};
                    string[] singlepalletsArray = new string[] { "OV", "PAL" };
                    if ((tagArray[index] == "Standard Security Service") || (tagArray[index] == "Dataserve" && itemtypeArray[index] != "TFT"))
                    {
                        if (withoutHDDArray.Contains(itemtypeArray[index].ToString()))
                        {
                            stdDisposalWithoutHDD++;
                            if(itemtypeArray[index] == "UPS")
                            {
                                stdDisposalWithoutHDD--;
                            }
                        }
                        else 
                        {
                            stdDisposalWithHDD++;

                        }
                    }// End - if(tagArray[index] == "Standard Secure Disposal")

                   if(modelArray[index].Contains("PLASMA"))
                   {
                       screenDisposalCount++;
                   }

                    //Haz Waste
                    if (tagArray[index] == "Haz Standard Security")
                    {
                        if (withoutHDDArray.Contains(itemtypeArray[index].ToString()))
                        {
                            highDisposalWithoutHDD++;
                        }
                        else if (withHDDArray.Contains(itemtypeArray[index].ToString()))
                        {
                            highDisposalWithHDD++;
                        }
                    }// End - if (tagArray[index] == "Haz Standard Security")
                   
                    //Screen
                    if (screenArray.Contains(itemtypeArray[index].ToString()))
                    {
                        screenDisposalCount++;
                    }
                    
                    //Pallet Count
                    if(singlepalletsArray.Contains(itemtypeArray[index].ToString()))
                    {
                        palletCount++;
                    }
                }
                //End of Disposal
            }
            //End of for (int index = 0; index < dt.Rows.Count; index++)


            //Start - Show values to user   

            //Redeployment
            PC5CountLabel.Text = Convert.ToString(PC5Count);
            PCLCountLabel.Text = Convert.ToString(PCLCount);
            PCSCountLabel.Text = Convert.ToString(PCSCount);
            LPCountLabel.Text = Convert.ToString(PrinterCount);
            TFTCountLabel.Text = Convert.ToString(TFTCount);
            OTCountLabel.Text = Convert.ToString(OTCount);
            cleanLabel.Text = Convert.ToString(CleanTest);
            pattestLabel.Text = Convert.ToString(PatTest);
            reboxLabel.Text = Convert.ToString(ReboxTest);
            printercleanLabel.Text = Convert.ToString(PrinterCount);

            //Sale
            saleTotalLabel.Text = "£ " + soldValueTotal.ToString();

            //Disposal
            stdDisposalWithHDDLabel.Text = Convert.ToString(stdDisposalWithHDD);
            stdDisposalWithoutHDDLabel.Text = Convert.ToString(stdDisposalWithoutHDD);
            highDisposalWithHDDLabel.Text = Convert.ToString(highDisposalWithHDD);
            highDisposalWithoutHDDLabel.Text = Convert.ToString(highDisposalWithoutHDD);
            screenDisposalCountLabel.Text = Convert.ToString(screenDisposalCount);
            palletcountLabel.Text = Convert.ToString(palletCount);
            
            //Totals
            redeployCount = PC5Count + PCLCount + PCSCount + PrinterCount + TFTCount + OTCount;
            disposalTotal = stdDisposalWithHDD + stdDisposalWithoutHDD + highDisposalWithHDD + highDisposalWithoutHDD + screenDisposalCount + palletCount;

            int itemsProcess = redeployCount + disposalTotal + soldCount;
            itemCountTotalLabel.Text = Convert.ToString(itemsProcess);

            //Records in Table
            int mytableRowCount = dt.Rows.Count;
            rowCountTotalLabel.Text = Convert.ToString(mytableRowCount);
            //End - Show values to user
        }
        //End of Dispatch Report




        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CollectionReportButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            Collection_Report cr = new Collection_Report();
            cr.Show();
        }

        private void StockReportButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            Stock_Report sr = new Stock_Report();
            sr.Show();
        }

        //Start of resetAll
        void resetAll()
        {
            PCLCountLabel.Text = "0";

            PC5CountLabel.Text = "0";

            LPCountLabel.Text = "0";

            TFTCountLabel.Text = "0";
 
            PCSCountLabel.Text = "0";

            OTCountLabel.Text = "0";


        }
        //end of resetAll


        //Start of not used section.
        public Dispatch_Report()
        {
            InitializeComponent();
        }

        private void dVLADBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.dVLADBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.iBill4meDataSet);
        }
        //end of not used section.
    }
}
