using SELKIE.Models;
using SELKIE.SimModels;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SELKIE
{
    public partial class Farm_Details : Form
    {
        delegate void SetStatusText(string _updatetext);
        public Farm_Details()
        {
            InitializeComponent();
            initFieldsFromObj();
        }

        private void initFieldsFromObj()
        {
            //read data from projectTab obj and update fileds accordingly
            textBoxmanufacturename.Text = FarmDetails.ManufatureName;
            textBoxtotalnumber.Text = FarmDetails.NoOfDivices.ToString();
            comboBoxTechnologyType.Text = FarmDetails.TechType;
            comboBoxSelectpowercurve.Text = FarmDetails.PowerCurve;
            textBoxSubstructrename.Text = FarmDetails.Substructrename;
            comboBoxSubStrucuturetype.Text = FarmDetails.Substructretype;
            textBoxDistancebetweendevices.Text = FarmDetails.Distancebetweendevices.ToString();

            tidalHubHeight.Text = FarmDetails.Tidal_hubHeight.ToString();
            tidalMeasurementH.Text = FarmDetails.Tidal_height.ToString();

            if (!string.IsNullOrEmpty(FarmDetails.ManufatureName))
                ValidateData();
        }

        private void pcuploadBtn_Click(object sender, EventArgs e)
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
                            FarmDetails.PowerCurve = ofd.FileName;
                            comboBoxSelectpowercurve.Text = FarmDetails.PowerCurve;
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

        private void SavFarmDetails_Click(object sender, EventArgs e)
        {
            ValidateData();
            _ = WeatherYearlyData.Reset();
            PowerCurveData.Reset();
        }

        private void ValidateData()
        {
            #region ManufatureName
            labelManufacturername.ForeColor = Color.Red;
            if (!string.IsNullOrEmpty(textBoxmanufacturename.Text))
            {
                //check length condition
                if (textBoxmanufacturename.Text.Length >= 1 && textBoxmanufacturename.Text.Length <= 50)
                {
                    FarmDetails.ManufatureName = textBoxmanufacturename.Text;
                    labelManufacturername.ForeColor = Color.Black;
                }
            }
            #endregion
            #region Totalnumber
            labeltotalnumber.ForeColor = Color.Red;
            if (int.TryParse(textBoxtotalnumber.Text, out int tno))
            {
                if (tno > 0)
                {
                    FarmDetails.NoOfDivices = tno;
                    labeltotalnumber.ForeColor = Color.Black;
                }

            }
            #endregion
            #region Technology type
            ftechtypeLbl.ForeColor = Color.Red;
            if (!string.IsNullOrEmpty(comboBoxTechnologyType.Text))
            {
                if (comboBoxTechnologyType.Text.Length >= 1 && comboBoxTechnologyType.Text.Length <= 50)
                {
                    FarmDetails.TechType = comboBoxTechnologyType.Text;
                    ftechtypeLbl.ForeColor = Color.Black;
                }
            }
            #endregion
            #region Substructre name
            structNameLbl.ForeColor = Color.Red;
            if (!string.IsNullOrEmpty(textBoxSubstructrename.Text))
            {
                if (textBoxSubstructrename.Text.Length >= 1 && textBoxSubstructrename.Text.Length <= 50)
                {
                    //update obj
                    FarmDetails.Substructrename = textBoxSubstructrename.Text;
                    structNameLbl.ForeColor = Color.Black;
                }
            }
            #endregion
            #region Substructre type
            structype.ForeColor = Color.Red;
            if (!string.IsNullOrEmpty(comboBoxSubStrucuturetype.Text))
            {
                FarmDetails.Substructretype = comboBoxSubStrucuturetype.Text;
                structype.ForeColor = Color.Black;
            }
            #endregion
            #region Distance between devices
            dbtndevicesLbl.ForeColor = Color.Red;

            if (double.TryParse(textBoxDistancebetweendevices.Text, out double dbdevices))
            {
                if (dbdevices > 0)
                {
                    FarmDetails.Distancebetweendevices = dbdevices;
                    dbtndevicesLbl.ForeColor = Color.Black;
                }
            }
            #endregion

            #region tidal or wave
            if (!string.IsNullOrEmpty(FarmDetails.TechType))
            {
                //wave PC section
                #region Select Powercurve
                label4selectpowercurvedata.ForeColor = Color.Red;
                if (!string.IsNullOrEmpty(comboBoxSelectpowercurve.Text))
                {
                    FarmDetails.PowerCurve = comboBoxSelectpowercurve.Text;
                    label4selectpowercurvedata.ForeColor = Color.Black;
                }
                #endregion

                #region tidalMeasurementH
                Tidal_heightLbl.ForeColor = Color.Red;
                if (double.TryParse(tidalMeasurementH.Text, out double _tidalMeasurementH))
                {
                    if (_tidalMeasurementH > 0)
                    {
                        FarmDetails.Tidal_height = _tidalMeasurementH;
                        Tidal_heightLbl.ForeColor = Color.Black;
                    }
                }
                #endregion

                #region tidalHubHeight
                tidalHubHeightLbl.ForeColor = Color.Red;
                if (double.TryParse(tidalHubHeight.Text, out double _tidalHubHeight))
                {
                    if (_tidalHubHeight > 0)
                    {
                        FarmDetails.Tidal_hubHeight = _tidalHubHeight;
                        tidalHubHeightLbl.ForeColor = Color.Black;
                    }
                }
                #endregion

            }
            #endregion
        }

        private void comboBoxTechnologyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxTechnologyType.SelectedIndex == 0)
            {
                tidalPanel.Visible = false;
            }
            else
            {
                tidalPanel.Visible = true;
            }
        }
    }
}
