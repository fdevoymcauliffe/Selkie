using SELKIE.Logic;
using SELKIE.Models;
using SELKIE.SimModels;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SELKIE
{
    public partial class Resource : Form
    {
        public ValidationCheck validCheck = new ValidationCheck();
        delegate void SetStatusText(string _updatetext);

        public Resource()
        {
            InitializeComponent();
            initFieldsFromObj();
        }

        private void initFieldsFromObj()
        {
            textBoxLocationname.Text = ResourceDetails.Locationname;
            textBoxWaterdepth.Text = ResourceDetails.Waterdepth.ToString();
            textBoxResourcedatameasurement.Text = ResourceDetails.Mesurementheight.ToString();
            comboBoxMetociandata.Text = ResourceDetails.Specifymetociandata;

            if (!string.IsNullOrEmpty(ResourceDetails.Locationname))
                CheckValidation();
        }

        private void wwuploadBtn_Click(object sender, EventArgs e)
        {
            try
            {
                //browse for file
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Excel(.xlsx)|*.xlsx|Excel(.xls)|*.xls";
                    ofd.Multiselect = false;
                    var result = ofd.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        try
                        {
                            ResourceDetails.Specifymetociandata = ofd.FileName;
                            comboBoxMetociandata.Text = ofd.FileName;

                            //File.Copy(ofd.FileName, DataFolders.WWFolder + "\\" + Path.GetFileName(ofd.FileName));
                            ////reload dropdown
                            //FillWWDataFiles();

                            ////select item from recent uploaded file
                            //ResourceDetails.Specifymetociandata = Path.GetFileNameWithoutExtension(ofd.FileName);
                            ////reload form
                            //comboBoxMetociandata.Text = ResourceDetails.Specifymetociandata;
                            //FillWWGridwithFiles();
                        }
                        catch (Exception)
                        {
                            //log 
                            _ = MessageBox.Show("File not suitable to upload. Please try again with right format.");
                            // log.Error(e);
                        }
                    }
                }
            }
            catch (Exception)
            {
                //log exception
                _ = MessageBox.Show("Error occured while saving file. Please try again or contact Admin");
            }

        }

        private void SaveResourceDetails_Click(object sender, EventArgs e)
        {
            CheckValidation();
            _ = WeatherYearlyData.Reset();
            PowerCurveData.Reset();
        }
        private void CheckValidation()
        {
            #region Location name
            labelLocationname.ForeColor = Color.Red;
            //check if string is not empty
            if (!string.IsNullOrEmpty(textBoxLocationname.Text))
            {
                //check length condition
                if (textBoxLocationname.Text.Length >= 1 && textBoxLocationname.Text.Length <= 50)
                {
                    ResourceDetails.Locationname = textBoxLocationname.Text;
                    labelLocationname.ForeColor = Color.Black;
                }
            }
            #endregion

            #region Water depth
            labelSitewaterdepth.ForeColor = Color.Red;
            double convertedValue = validCheck.CheckIfDouble(textBoxWaterdepth.Text);
            if (convertedValue >= 0)
            {
                //update obj
                ResourceDetails.Waterdepth = convertedValue;
                labelSitewaterdepth.ForeColor = Color.Black;
            }
            #endregion

            #region Resource mesurement
            labelMeasurementheight.ForeColor = Color.Red;
            double convertedValue2 = validCheck.CheckIfDouble(textBoxResourcedatameasurement.Text);
            if (convertedValue2 >= 0)
            {
                //update obj
                ResourceDetails.Mesurementheight = convertedValue2;
                labelMeasurementheight.ForeColor = Color.Black;
            }
            #endregion

            #region Specify metocian data
            label4Selectweatherdata.ForeColor = Color.Red;
            //check if string is not empty
            if (!string.IsNullOrEmpty(comboBoxMetociandata.Text))
            {
                //check length condition
                if (comboBoxMetociandata.Text.Length >= 1)
                {
                    //update obj
                    ResourceDetails.Specifymetociandata = comboBoxMetociandata.Text;
                    label4Selectweatherdata.ForeColor = Color.Black;
                }
            }
            #endregion
        }

    }
}
