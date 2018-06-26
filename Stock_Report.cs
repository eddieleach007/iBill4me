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
    public partial class Stock_Report : Form
    {
        //Start - my Variables.
        //item Count.
        int PCLCount = 0;
        int PC5Count = 0;
        int TFTCount = 0;
        int LPCount = 0;
        int PCSCount = 0;
        int HDDCount = 0;
        int OTCount = 0;
        int OVCount = 0;
        int datawipeCount = 0;
        int addHDDCount = 0;
        int itemtotal = 0;
        int itemsnotbilled = 0;

        //total results
        double pclTotal = 0;
        double pc5Total = 0;
        double lpTotal = 0;
        double tftTotal = 0;
        double pcsTotal = 0;
        double otTotal = 0;
        int itemsnotbilledTotal = 0;
        public string myDate;
        int tableSize; //used in DVLAS
        public string startDate;
        public string endDate;
        DateTime startbillingdate; //used in DVLAS
        //End - my Variables.

        private void Stock_Report_Load(object sender, EventArgs e)
        {
            this.dvlasTableAdapter1.Fill(this.iBill4meDataSet.DVLAS);
            startDateLabel.Text = startDate;
            endDateLabel.Text = endDate;
        }


        public Stock_Report()
        {
            InitializeComponent();
        }

        //Start of StockReportButton
        private void StockReportButton_Click(object sender, EventArgs e)
        {
            resetAll();
            this.iBill4meDataSet.Clear();
            myHeaderLabel.Text = "DVLA STOCK REPORT";
            //Getting the connection
            SqlConnection c = new SqlConnection(Properties.Settings.Default.iBill4meConnectionString);
            c.Open();
            
            SqlDataAdapter a = new SqlDataAdapter("SELECT * FROM DVLAS", c);
            DataTable dt = this.iBill4meDataSet.DVLAS;
            tableSize = dt.Rows.Count;
            
            a.Fill(dt);

            c.Close();
            //End of Getting connection
            string[] barcodeArray = new string[dt.Rows.Count];
            string[] collectionrefArray = new string[dt.Rows.Count];
            string[] collectiondateArray = new string[dt.Rows.Count];
            string[] manufacturerArray = new string[dt.Rows.Count];
            string[] itemtypeArray = new string[dt.Rows.Count];
            string[] modelArray = new string[dt.Rows.Count];
            string[] desriptionArray = new string[dt.Rows.Count];
            string[] gradeArray = new string[dt.Rows.Count];
            string[] locationArray = new string[dt.Rows.Count];
            string[] tagArray = new string[dt.Rows.Count];
            string[] serialnoArray = new string[dt.Rows.Count];
            string[] assetnoArray = new string[dt.Rows.Count];
            string[] assetno2Array = new string[dt.Rows.Count];
            string[] assetno3Array = new string[dt.Rows.Count];
            string[] assetno4Array = new string[dt.Rows.Count];
            string[] ordernoArray = new string[dt.Rows.Count];
            string[] extrabarcodeArray = new string[dt.Rows.Count];
            string[] bookedindateArray = new string[dt.Rows.Count];

            string col1 = "Barcode";
            string col2 = "Collection No";
            string col3 = "Collection Date";
            string col4 = "Manufacturer";
            string col5 = "Item Type";
            string col6 = "Model";
            string col7 = "Description";
            string col8 = "Grade";
            string col9 = "Location";
            string col10 = "Tag";
            string col11 = "Serial No#";
            string col12 = "Asset No#";
            string col13 = "Asset No2#";
            string col14 = "Asset No3#";
            string col15 = "Asset No4#";
            string col16 = "Order No#";
            string col17 = "Extra Barcode";
            string col18 = "Booked In Date";

            for (int index = 0; index < dt.Rows.Count; index++)
            {//Start - for(int index = 0; index < dt.Rows.Count; index++)
                barcodeArray[index] = dt.Rows[index][col1].ToString();
                collectionrefArray[index] = dt.Rows[index][col2].ToString();
                collectiondateArray[index] = dt.Rows[index][col3].ToString();
                manufacturerArray[index] = dt.Rows[index][col4].ToString();
                itemtypeArray[index] = dt.Rows[index][col5].ToString();
                modelArray[index] = dt.Rows[index][col6].ToString();
                desriptionArray[index] = dt.Rows[index][col7].ToString();
                gradeArray[index] = dt.Rows[index][col8].ToString();
                locationArray[index] = dt.Rows[index][col9].ToString();
                tagArray[index] = dt.Rows[index][col10].ToString();
                serialnoArray[index] = dt.Rows[index][col11].ToString();
                assetnoArray[index] = dt.Rows[index][col12].ToString();
                assetno2Array[index] = dt.Rows[index][col13].ToString();
                assetno3Array[index] = dt.Rows[index][col14].ToString();
                assetno4Array[index] = dt.Rows[index][col15].ToString();
                ordernoArray[index] = dt.Rows[index][col16].ToString();
                extrabarcodeArray[index] = dt.Rows[index][col17].ToString();
                bookedindateArray[index] = dt.Rows[index][col18].ToString();
                //Data Conversations take place here.

                DateTime checkingDateValue = Convert.ToDateTime(bookedindateArray[index]);
                DateTime tempstartdate = Convert.ToDateTime(startDate);
                string[] nobilltagitemArray = new string[] { "Sale", "In Receipt", "Audited" };
                string[] OTitemArray = new string[] { "SWI", "OT", "OD", "CAB", "SCA", "FAX", "TOU", "DOC", "ROU", "MEM", "HUB", "UPS", "MOD" };
                string[] OVitemArray = new string[] { "OV", "PAL"};

                //MFP located at row Z6ROW1 need to be billed as well.
                //OV and PAL need to included seperately. Move MFP & PAL into OV catagory.

                if (checkingDateValue < tempstartdate)
                {//Earlier dates then the value entered will get billed.
                    //MessageBox.Show("Current Barcode: " + barcodeArray[index].ToString());
                    if (ordernoArray[index] != "")
                    {//If no order number this gets billed.              
                        itemsnotbilled++;
                    }//End - (ordernoArray[index] == !") 
                    else //(ordernoArray[index] == !") 
                    {
                        //MessageBox.Show("Gets billed: No Order Number. " + barcodeArray[index].ToString() + " " + ordernoArray[index].ToString());
                        if (nobilltagitemArray.Contains(tagArray[index].ToString()))
                        {
                            itemsnotbilled++;
                        }
                        else 
                        {
                            if (itemtypeArray[index] == "PC5")
                            {
                                PC5Count++;
                            }
                            else if (itemtypeArray[index] == "PCL")
                            {
                                PCLCount++;
                            }
                            else if (itemtypeArray[index] == "PCS")
                            {
                                PCSCount++;                            
                            }
                            else if ((itemtypeArray[index] == "TFT") || (itemtypeArray[index] == "PCM"))
                            {
                                TFTCount++;
                            }
                            else if ((itemtypeArray[index] == "LP") || (itemtypeArray[index] == "MFP"))
                            {
                                LPCount++;
                            }
                           else if (OTitemArray.Contains(itemtypeArray[index].ToString()))
                            {
                                OTCount++;        
                            }
                            else if (OVitemArray.Contains(itemtypeArray[index].ToString()))
                            {
                                OVCount++;
                            }
                            else if(itemtypeArray[index] == "HDD")
                            {
                                if((assetnoArray[index] == "NO PARENT SYSTEM") || extrabarcodeArray[index] == "NO PARENT SYS")
                                {
                                    HDDCount++;                                
                                }
                            
                            }
                        }
                    }
                }//End - (checkingDateValue < startbillingdate)
                else
                {
                    itemsnotbilled++;
                }
            }//End - for(int index = 0; index < dt.Rows.Count; index++)

            
            
            int PC5CountTotal = PC5Count;
            int PCLCountTotal = PCLCount;
            int LPCountTotal = LPCount;
            int TFTCountTotal = TFTCount;
            int OTCountTotal = OTCount;
            int OVCountTotal = OVCount;

            int HDDCountTotal = HDDCount;

            pc5CountLabel.Text = PC5Count.ToString();
            pclCountLabel.Text = PCLCount.ToString();
            lpCountLabel.Text = LPCount.ToString();
            tftLabelCount.Text = TFTCount.ToString();
            OTCountLabel.Text = OTCount.ToString();
            OVCountLabel.Text = OVCount.ToString();

            hddCountLabel.Text = HDDCountTotal.ToString();

            int itemCountTotal = PC5CountTotal + PCLCountTotal + LPCountTotal + TFTCountTotal + OTCountTotal + OVCountTotal + HDDCountTotal;

            itemsnotbilledTotal = itemsnotbilledTotal + itemsnotbilled;
            itemsnotbilledLabel.Text = Convert.ToString(itemsnotbilledTotal);

            int mytableRowCount = dt.Rows.Count;
            itemCountTotalLabel.Text = itemCountTotal.ToString();
            rowCountTotalLabel.Text = Convert.ToString(mytableRowCount);
        }
        //End of StockReportButton

        //Start of Collection Report Button
        private void CollectionReportButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            Collection_Report cr = new Collection_Report();
            cr.Show();
        }
        //End of Collection Report Button

        private void DispatchReportButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            Dispatch_Report dr = new Dispatch_Report();
            dr.Show();
        }


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
            itemtotal = 0;
            
        }
        //end of resetAll

        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dVLASBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.dVLASBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.iBill4meDataSet);

        }




        
    }
}


           