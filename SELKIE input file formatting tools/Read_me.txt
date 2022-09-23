This folder contains 2 tools to help users format their inputs for the SELKIE O&M and logistics model including:
a) power matrix and 
b) metocean data time series 

Origin
These tools were created in Matlab R2015a and compiled on 26th August 2022 by Fiona Devoy McAuliffe (University College Cork).
Any questions regarding the formatting tools can be directed to f.devoymcauliffe@ucc.ie

System requirements
The tools were developed in matlab and compiled into an executable that does not require matlab software to use. However, in order to run any Matlab program on a machine that
doesn't have Matlab installed you will need to install the Matlab Compiler Runtime (MCR). These tools were developed using Matlab R2015a (8.5) and will therefore require installation 
of the corresponding (free) runtime software via https://uk.mathworks.com/products/compiler/matlab-runtime.html.

The user also needs microsoft office excel to provide the input files.

To use...
1) Input your metocean data and/or power matrix in the respective input excel file "Metocean_data_input.xlsx" and "PM_input.xlsx".

Sample data has been included based on 
a) the Pelamis P2 power matrix provided in A. Gray, B. Dickens, T. Bruce, I. Ashton and L. Johanning, “Reliability and O &M sensitivity analysis as a consequence of site specific 
characteristics for wave energy converters,” Ocean Engineering, vol. 141, pp. 493-511, 2017.  
b) metocean data generated for the FarrPoint site provided in the Wave Energy Scotland OM tool 
(see library.waveenergyscotland.co.uk/other-activities/design-tools-and-information/tools/om-simulation-tool/)

2) Select the metocean data or power matrix executable as relevant and wait for the progress bar to close. 
The power matrix model should only take a few seconds to complete while the metocean data will depend on the number of years provided and may take a few minutes.