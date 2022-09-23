using SELKIE.Entities;
using SELKIE.Models;
using System;
using System.IO;
using System.Linq;

namespace SELKIE.Logic
{
    public class ValidationCheck
    {

        public ValidationCheck()
        {

        }
        /// <summary>
        /// This method is to check if entered value is int
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int CheckIfInt(string value)
        {
            int check = -1;
            try
            {

                if (Int32.TryParse(value, out check))
                {
                    return check;
                }
                else
                    check = -1;
            }
            catch (Exception)
            {
                //log issue
            }
            return check;
        }
        public double CheckIfDouble(string value)
        {
            double check = -1;
            try
            {

                if (double.TryParse(value, out check))
                {
                    return check;
                }
                else
                    check = -1;
            }
            catch (Exception)
            {
                //log issue
            }
            return check;
        }
        public ValidationMessage CheckProjectValidation()
        {
            ValidationMessage result = new ValidationMessage
            {
                Result = true,
                Message = "Project validations: "
            };
            try
            {
                //check project name
                if (string.IsNullOrEmpty(ProjectDetails.ProjectName))
                {
                    result.Result = false;
                    result.Message += $"Project name, ";
                }
                //check project installation time
                int convertedValue = CheckIfInt(ProjectDetails.InstallTime.ToString());
                if (convertedValue <= 0)
                {
                    result.Result = false;
                    result.Message += $"Project installation time, ";
                }
                //project life time
                int lifeTCheck = CheckIfInt(ProjectDetails.ProjectLifeTime.ToString());
                if (lifeTCheck < 0)
                {
                    result.Result = false;
                    result.Message += $" Project life time, ";
                }
                //project no of iterations
                int noofIterations = CheckIfInt(ProjectDetails.NoOfIterations.ToString());
                if (noofIterations <= 0)
                {
                    result.Result = false;
                    result.Message += $" Project no of iteration, ";
                }
                //Grid sale rate
                double gridsalerate = CheckIfDouble(ProjectDetails.GridSaleRate.ToString());
                if (gridsalerate < 0)
                {
                    result.Result = false;
                    result.Message += $" Grid sale rate, ";
                }
                //Wake losses
                double wakeLoses = CheckIfDouble(ProjectDetails.WakeLoss.ToString());
                if (wakeLoses < 0)
                {
                    result.Result = false;
                    result.Message += $" Wake loses, ";
                }
                //Transmisson Losses
                double transmissonLosses = CheckIfDouble(ProjectDetails.TransLoss.ToString());
                if (transmissonLosses < 0)
                {
                    result.Result = false;
                    result.Message += $" Transmission losses, ";
                }
                //Other losses
                double otherLosses = CheckIfDouble(ProjectDetails.ProdLoss.ToString());
                if (otherLosses < 0)
                {
                    result.Result = false;
                    result.Message += $" Other losses, ";
                }

                return result;
            }
            catch (Exception)
            {

            }
            return new ValidationMessage();
        }
        public ValidationMessage CheckFarmValidation()
        {
            ValidationMessage result = new ValidationMessage
            {
                Result = true,
                Message = "Farm validations: "
            };
            try
            {
                //Check Manufacture name
                if (string.IsNullOrEmpty(FarmDetails.ManufatureName))
                {
                    result.Result = false;
                    result.Message += $" Manufacture name, ";
                }
                //Total number
                int totalNumber = CheckIfInt(FarmDetails.NoOfDivices.ToString());
                if (totalNumber <= 0)
                {
                    result.Result = false;
                    result.Message += $"Total number, ";
                }
                //Technolgy type
                if (string.IsNullOrEmpty(FarmDetails.TechType))
                {
                    result.Result = false;
                    result.Message += $"Technolgy type, ";
                }

                //Power matrix/Curve
                if (string.IsNullOrEmpty(FarmDetails.PowerCurve))
                {
                    //result.Result = false;
                    result.Message += $"Power matrix, ";
                }
                else
                {
                    //check if file exists
                    try
                    {
                        if (!File.Exists(FarmDetails.PowerCurve))
                        {
                            result.FileExists = false;
                            result.Result = true;
                            result.Message += $" Specify power curve or matrix data file (" + FarmDetails.PowerCurve + ") is missing, ";
                        }
                    }
                    catch (Exception)
                    {
                        result.Result = false;
                        result.Message += $" Specify power curve or matrix data file (" + FarmDetails.PowerCurve + ") is missing, ";
                    }


                }

                if (FarmDetails.TechType.ToLower() != "wave")
                {
                    double tidalHubHeight = CheckIfDouble(FarmDetails.Tidal_hubHeight.ToString());
                    if (tidalHubHeight <= 0)
                    {
                        result.Result = false;
                        result.Message += $"Hub height, ";
                    }

                    double tidalMeasurementH = CheckIfDouble(FarmDetails.Tidal_height.ToString());
                    if (tidalMeasurementH <= 0)
                    {
                        result.Result = false;
                        result.Message += $"Measurement height, ";
                    }
                }

                //Substructure name
                if (string.IsNullOrEmpty(FarmDetails.Substructrename))
                {
                    result.Result = false;
                    result.Message += $"Substructure name, ";
                }
                //Substructre type
                if (string.IsNullOrEmpty(FarmDetails.Substructretype))
                {
                    result.Result = false;
                    result.Message += $"Substructre type, ";
                }
                //Distance between devices
                double distancebetweenDevices = CheckIfDouble(FarmDetails.Distancebetweendevices.ToString());
                if (distancebetweenDevices < 0)
                {
                    result.Result = false;
                    result.Message += $"Distance between devices, ";
                }
                return result;
            }
            catch (Exception)
            {

            }
            return new ValidationMessage();
        }
        public ValidationMessage CheckResourceValidation()
        {
            ValidationMessage result = new ValidationMessage
            {
                Result = true,
                Message = "Resource validations: "
            };
            try
            {
                //location name
                if (string.IsNullOrEmpty(ResourceDetails.Locationname))
                {
                    result.Result = false;
                    result.Message += $" Location name, ";
                }
                //Water depth
                double waterDepth = CheckIfDouble(ResourceDetails.Waterdepth.ToString());
                if (waterDepth < 0)
                {
                    result.Result = false;
                    result.Message += $" Water depth, ";
                }
                //Resource mesurement data
                double mesurementData = CheckIfDouble(ResourceDetails.Mesurementheight.ToString());
                if (mesurementData < 0)
                {
                    result.Result = false;
                    result.Message += $" Resource mesurement data, ";
                }

                //Sfecify metocian data
                if (string.IsNullOrEmpty(ResourceDetails.Specifymetociandata))
                {
                    //result.Result = false;
                    result.Message += $" Specify metocian data, ";
                }
                else
                {
                    //check if file exists
                    try
                    {
                        if (!File.Exists(ResourceDetails.Specifymetociandata))
                        {
                            result.FileExists = false;
                            result.Result = true;
                            result.Message += $" Specify metocian data file (" + ResourceDetails.Specifymetociandata + ") is missing, ";
                        }
                    }
                    catch (Exception)
                    {
                        result.Result = false;
                        result.Message += $" Specify metocian data file (" + ResourceDetails.Specifymetociandata + ") is missing, ";
                    }
                }

                return result;
            }
            catch (Exception)
            {

            }
            return new ValidationMessage();
        }
        public ValidationMessage CheckBasesValidation()
        {
            ValidationMessage result = new ValidationMessage
            {
                Result = true,
                Message = "Bases validations: "
            };
            try
            {
                //get list of bases
                var allBases = TotalBases.GetBases();
                if (allBases.Any())
                {
                    int repairIndex = 1;
                    foreach (var _base in allBases)
                    {
                        //check basename
                        if (string.IsNullOrEmpty(_base.Basename))
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Base name, ";
                        }
                        //Annual running cost
                        double convertedValue1 = CheckIfDouble(_base.Annualcost.ToString());
                        if (convertedValue1 < 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Annual running cost, ";
                        }
                        //Distance to farm
                        double convetedValue2 = CheckIfDouble(_base.Distancetofarm.ToString());
                        if (convetedValue2 <= 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Distance to form, ";
                        }
                        //No of techs
                        int convertedValue3 = CheckIfInt(_base.NoOfTechs.ToString());
                        if (convertedValue3 <= 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) No of techs, ";
                        }
                        //Annual salary for tech
                        double convertedValue4 = CheckIfDouble(_base.AnnualsalperTech.ToString());
                        if (convertedValue4 < 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Annual salary for tech, ";
                        }
                        repairIndex++;
                    }
                }
                else
                    return result;

                return result;
            }
            catch (Exception)
            {

            }
            return new ValidationMessage();
        }
        public ValidationMessage CheckVesselsValidation()
        {
            ValidationMessage result = new ValidationMessage
            {
                Result = true,
                Message = "Vessels validations: "
            };
            try
            {
                //get list of vessels
                var allVessels = TotalVessels.GetVessels();
                if (allVessels.Any())
                {
                    int repairIndex = 1;
                    foreach (var _vessels in allVessels)
                    {
                        //check vesselname
                        int convertedValue = CheckIfInt(_vessels.VesselClassif);
                        if (convertedValue > 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Vessel name, ";
                        }
                        //Number
                        int convertedValue1 = CheckIfInt(_vessels.Number.ToString());
                        if (convertedValue1 <= 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Number, ";
                        }
                        //Tech Capacity
                        int convertedValue2 = CheckIfInt(_vessels.TechsCapacity.ToString());
                        if (convertedValue2 <= 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Tech capacity, ";
                        }
                        //Night work
                        int convertedValue3 = CheckIfInt(_vessels.NightWork);
                        if (convertedValue3 > 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Night work, ";
                        }

                        if (_vessels.Purchased)
                        {
                            //Annual running cost
                            double convertedValue4 = CheckIfDouble(_vessels.AnnualrunningCost.ToString());
                            if (convertedValue4 < 0)
                            {
                                result.Result = false;
                                result.Message += $"({repairIndex}) Annual running cost, ";
                            }
                        }


                        if (!_vessels.Purchased)
                        {
                            if (_vessels.Hireasrequired.ToLower() == "no")
                            {
                                //Rental start Day
                                int convertedValue5 = CheckIfInt(_vessels.RentalStartDay.ToString());
                                if (convertedValue5 <= 0)
                                {
                                    result.Result = false;
                                    result.Message += $"({repairIndex}) Rental start day, ";
                                }
                                //Rental End Day
                                int convertedValue6 = CheckIfInt(_vessels.RentalEndDay.ToString());
                                if (convertedValue6 <= 0)
                                {
                                    result.Result = false;
                                    result.Message += $"({repairIndex}) Rental end day, ";
                                }
                                //Rental start month
                                int convertedValue7 = CheckIfInt(_vessels.RentalStartMonth.ToString());
                                if (convertedValue7 <= 0)
                                {
                                    result.Result = false;
                                    result.Message += $"({repairIndex}) Rental start month, ";
                                }
                                //Rental end month
                                int convertedValue8 = CheckIfInt(_vessels.RentalEndMonth.ToString());
                                if (convertedValue8 <= 0)
                                {
                                    result.Result = false;
                                    result.Message += $"({repairIndex}) Rental end month, ";
                                }
                                //Daily rental cost
                                double convertedValue9 = CheckIfDouble(_vessels.DailyRentalCost.ToString());
                                if (convertedValue9 < 0)
                                {
                                    result.Result = false;
                                    result.Message += $"({repairIndex}) Rental start month cost, ";
                                }
                            }
                        }

                        //Mobilization cost
                        double convertedValue10 = CheckIfDouble(_vessels.MobilizationCost.ToString());
                        if (convertedValue10 < 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Mobilization cost, ";
                        }
                        //Fuel consumption
                        double convertedValue11 = CheckIfDouble(_vessels.FuelConsumption.ToString());
                        if (convertedValue11 < 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Fuel consumption, ";
                        }
                        //Fuel cost
                        double convertedValue12 = CheckIfDouble(_vessels.FuelCost.ToString());
                        if (convertedValue12 < 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex})Fuel cost, ";
                        }
                        //Speed
                        double convertedValue13 = CheckIfDouble(_vessels.Speed.ToString());
                        if (convertedValue13 < 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Speed, ";
                        }
                        //Hired as required
                        int convertedValue14 = CheckIfInt(_vessels.Hireasrequired);
                        if (convertedValue14 > 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Hired as required, ";
                        }
                        //Vessel lead time
                        double convertedValue15 = CheckIfDouble(_vessels.VesselLeadtime.ToString());
                        if (convertedValue15 < 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Vessel lead time, ";
                        }

                    }

                }
                else
                    return result;

                return result;
            }
            catch (Exception)
            {


            }
            return new ValidationMessage();
        }
        public ValidationMessage ChekInstallationValidation()
        {
            ValidationMessage result = new ValidationMessage
            {
                Result = true,
                Message = "Installation validations: "
            };
            try
            {
                //get list of Instals
                var allInstalls = TotalInstallations.GetInstalls();
                if (allInstalls.Any())
                {
                    if (allInstalls.Count < 2)
                    {
                        return new ValidationMessage
                        {
                            Message = "Check Installation required data",
                            Result = false
                        };
                    }


                    int repairIndex = 1;
                    foreach (var _instals in allInstalls)
                    {
                        //check Task name
                        int convertedValue = CheckIfInt(_instals.Taskname);
                        if (convertedValue > 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Task name, ";
                        }
                        //Taskdescription name
                        int convertedValue1 = CheckIfInt(_instals.TaskDescription);
                        if (convertedValue1 > 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Task description , ";
                        }
                        //Number of techs required
                        int convertedValue2 = CheckIfInt(_instals.NoOftechsReq.ToString());
                        if (convertedValue2 <= 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Number of techs, ";
                        }
                        //Vessele required
                        int convertedValue3 = CheckIfInt(_instals.VesselReq);
                        if (convertedValue3 > 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Vessele required, ";
                        }
                        //Number of vessels required for device
                        int convertedValue4 = CheckIfInt(_instals.Numberofdevicespervessel.ToString());
                        if (convertedValue4 <= 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Vessels required for device, ";
                        }
                        //Base name
                        int convertedValue5 = CheckIfInt(_instals.Base);
                        if (convertedValue5 > 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex})Base name, ";
                        }
                        //Operation duration
                        double convertedValue6 = CheckIfDouble(_instals.OperationDuration.ToString());
                        if (convertedValue6 < 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Operation duration, ";
                        }
                        //Wave height limit
                        double convertedValue7 = CheckIfDouble(_instals.Waveheightlimit.ToString());
                        if (convertedValue7 < 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Waveheight limit, ";
                        }
                        //Wave period limit
                        double convertedValue8 = CheckIfDouble(_instals.Waveperiodlimit.ToString());
                        if (convertedValue8 < 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Waveperiod limit, ";
                        }
                        //Wind speed limit
                        double convertedValue9 = CheckIfDouble(_instals.Windspeedlimit.ToString());
                        if (convertedValue9 < 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Windspeed limit, ";
                        }
                        //Current velocity
                        double convertedValue10 = CheckIfDouble(_instals.Currentvelocitylimit.ToString());
                        if (convertedValue10 < 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Current velocity, ";
                        }
                        //check if selected vessel exsits
                        if (TotalVessels.GetObjByName(_instals.VesselReq) == null)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Vessel required, ";
                        }
                        repairIndex++;
                    }
                }
                else
                    return new ValidationMessage
                    {
                        Message = "Check Installation required data",
                        Result = false
                    };

                return result;

            }
            catch (Exception)
            {


            }
            return new ValidationMessage();
        }
        public ValidationMessage CheckPMValidation()
        {
            ValidationMessage result = new ValidationMessage
            {
                Result = true,
                Message = "PM validations: "
            };
            try
            {
                //get list of Priventives
                var allPriventives = TotalPriventives.GetAllPriventives();
                if (allPriventives.Any())
                {
                    int repairIndex = 1;
                    foreach (var _priventives in allPriventives)
                    {
                        //check PM Category
                        int convertedValue = CheckIfInt(_priventives.PMCategory);
                        if (convertedValue > 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) PM Category, ";
                        }
                        //Task description
                        int convertedValue1 = CheckIfInt(_priventives.Taskdescription);
                        if (convertedValue1 > 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Task description, ";
                        }
                        //Number technicians
                        int convertedValue2 = CheckIfInt(_priventives.NoOftechsReq.ToString());
                        if (convertedValue2 <= 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Number of techs, ";
                        }
                        //Vessel required
                        int convertedValue3 = CheckIfInt(_priventives.VesselReq);
                        if (convertedValue3 > 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Vessel required, ";
                        }
                        //Select base
                        int convertedValue4 = CheckIfInt(_priventives.VesselReq);
                        if (convertedValue4 > 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Select base, ";
                        }
                        //Frequency
                        double convertedValue5 = CheckIfDouble(_priventives.Frequency.ToString());
                        if (convertedValue5 < 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Frequency, ";
                        }
                        //Operation Location
                        int convertedValue6 = CheckIfInt(_priventives.OperationLOC.ToString());
                        if (convertedValue6 > 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Operation location, ";
                        }
                        //Operation duration offshore
                        double convertedValue7 = CheckIfDouble(_priventives.OprDurationOffs.ToString());
                        if (convertedValue7 < 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Offshore, ";
                        }
                        //Operation duration onshore
                        double convertedValue8 = CheckIfDouble(_priventives.OprDurationOns.ToString());
                        if (convertedValue8 < 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Onshore, ";
                        }
                        //Wave height limit
                        double convertedValue9 = CheckIfDouble(_priventives.Waveheightlimit.ToString());
                        if (convertedValue9 < 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Waveheight limit, ";
                        }
                        //Wave period limit
                        double convertedValue10 = CheckIfDouble(_priventives.Waveperiodlimit.ToString());
                        if (convertedValue10 < 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Wave period limit, ";
                        }
                        //Wind speed limit
                        double convertedValue11 = CheckIfDouble(_priventives.Windspeedlimit.ToString());
                        if (convertedValue11 < 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Windspeed limit, ";
                        }
                        //Current velocity
                        double convertedValue12 = CheckIfDouble(_priventives.CurrentVelocityLimit.ToString());
                        if (convertedValue12 < 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Current velocity, ";
                        }
                        //Power loss
                        double convertedValue13 = CheckIfDouble(_priventives.Powerloss.ToString());
                        if (convertedValue13 < 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Power loss, ";
                        }
                        //Spare cost
                        double convertedValue14 = CheckIfDouble(_priventives.Sparepart.ToString());
                        if (convertedValue14 < 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Spare cost, ";
                        }
                        //check if selected base exsits
                        if (TotalBases.GetObjByName(_priventives.Base) == null)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Base required, ";
                        }
                        //check if selected vessel exsits
                        if (TotalVessels.GetObjByName(_priventives.VesselReq) == null)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Vessel required, ";
                        }
                        repairIndex++;


                    }
                }
                else
                    return result;
                return result;
            }
            catch (Exception)
            {


            }
            return new ValidationMessage();
        }
        public ValidationMessage CheckRepairsValidation()
        {
            ValidationMessage result = new ValidationMessage
            {
                Result = true,
                Message = "Repair validations: "
            };
            try
            {
                //get list of Repairs
                var allRepairs = TotalRepairs.GetAllRepairs();
                if (allRepairs.Any())
                {
                    int repairIndex = 1;
                    foreach (var _repairs in allRepairs)
                    {
                        //check Repair name
                        int convertedValue = CheckIfInt(_repairs.RepairName);
                        if (convertedValue > 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Name, ";
                        }
                        //Repair description
                        int convertedValue1 = CheckIfInt(_repairs.RepairDesc);
                        if (convertedValue1 > 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Repair Description, ";
                        }
                        //Number of technicians required
                        int convertedValue2 = CheckIfInt(_repairs.NoOfTechs.ToString());
                        if (convertedValue2 <= 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) No of Techs, ";
                        }
                        //Vessel required 
                        int convertedValue3 = CheckIfInt(_repairs.Vesselrequired);
                        if (convertedValue3 > 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Vessel required, ";
                        }
                        //Base
                        int convertedValue4 = CheckIfInt(_repairs.Base);
                        if (convertedValue4 > 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Select base, ";
                        }
                        //Operation location
                        int convertedValue5 = CheckIfInt(_repairs.Operationlocation.ToString());
                        if (convertedValue5 > 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Operation location, ";
                        }
                        //Opr duration offshore
                        double convertedValue6 = CheckIfDouble(_repairs.OperationdurationOffshore.ToString());
                        if (convertedValue6 <= 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Operation duraton offshore, ";
                        }

                        if (_repairs.Operationlocation == Enums.OperationalLocation.Onshore)
                        {
                            //Opr onshore duration onshore
                            double convertedValue7 = CheckIfDouble(_repairs.OperatondurationOnshore.ToString());
                            if (convertedValue7 <= 0)
                            {
                                result.Result = false;
                                result.Message += $"({repairIndex}) Operation duraton onshore, ";
                            }
                        }

                        //Wave height limit
                        double convertedValue8 = CheckIfDouble(_repairs.Waveheightlimit.ToString());
                        if (convertedValue8 <= 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Waveheight limit, ";
                        }
                        //Wave period limit
                        double convertedValue9 = CheckIfDouble(_repairs.Waveperiodlimit.ToString());
                        if (convertedValue9 <= 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Waveperiod limit, ";
                        }
                        //Wind speed limit
                        double convertedValue10 = CheckIfDouble(_repairs.Windspeedlimit.ToString());
                        if (convertedValue10 <= 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Wind speed limit, ";
                        }
                        //Current velocity limit
                        double convertedValue11 = CheckIfDouble(_repairs.Currentvelocitylimit.ToString());
                        if (convertedValue11 < 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Current velocity limit, ";
                        }
                        //Power loss
                        double convertedValue12 = CheckIfDouble(_repairs.Powerloss.ToString());
                        if (convertedValue12 < 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Power loss, ";
                        }

                        //check if selected base exsits
                        if (TotalBases.GetObjByName(_repairs.Base) == null)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Base required, ";
                        }
                        //check if selected vessel exsits
                        if (TotalVessels.GetObjByName(_repairs.Vesselrequired) == null)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Vessel required, ";
                        }

                        repairIndex++;
                    }
                }
                else
                    return result;
                return result;
            }
            catch (Exception)
            {


            }
            return new ValidationMessage();
        }
        public ValidationMessage CheckComponentsValidation()
        {
            ValidationMessage result = new ValidationMessage
            {
                Result = true,
                Message = "Components validations: "
            };
            try
            {
                //get list of Components
                var allComps = TotalComponents.GetAllComponents();
                if (allComps.Any())
                {
                    int repairIndex = 1;
                    foreach (var _comps in allComps)
                    {
                        //check Component name
                        int convertedValue = CheckIfInt(_comps.Componentname);
                        if (convertedValue > 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Component name, ";
                        }
                        //Number per devices
                        int convertedValue1 = CheckIfInt(_comps.Numberperdevice.ToString());
                        if (convertedValue1 <= 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Number per device, ";
                        }
                        //Annual failure rate
                        double convertedValue2 = CheckIfDouble(_comps.AnnualFailRate.ToString());
                        if (convertedValue2 <= 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Annual failure rate, ";
                        }
                        //Repair category
                        int convertedValue3 = CheckIfInt(_comps.Repair);
                        if (convertedValue3 > 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Repair category, ";
                        }
                        //Spare part
                        double convertedValue4 = CheckIfDouble(_comps.SpareParts.ToString());
                        if (convertedValue4 < 0)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Spare part, ";
                        }
                        //check if selected Repair Category exits
                        if (TotalRepairs.GetObjByName(_comps.Repair) == null)
                        {
                            result.Result = false;
                            result.Message += $"({repairIndex}) Repair required, ";
                        }
                        repairIndex++;

                    }
                }
                else
                    return result;
                return result;
            }
            catch (Exception)
            {


            }
            return new ValidationMessage();
        }

    }
}

