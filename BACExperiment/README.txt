BACExperiment 

The BACExperiment software is intended for the Behaviour And Cognition Lab of Aarhus University . 

Developer : Iustinian Olaru
Managed : Dan Monster












Update Log :

1.21.08.14 - Wii remotes functional . Sensor screen in GUI is responsive with the wii remotes movements . For some reason tho the firs Wii remote has the horizontal axis inverter . 
             Elipses in StimulyWindow are responsive to the movements of the remotes. Ellipses move in skipping frames . Looking into threads to see if putting the two remote objec
			 in separate threads would improve the ellipse responsiveness. 
              
1.19.08.14 - Code refactored again , moved methods around in the classes with the purpose of grouping them .

1.18.08.14_02 - Refactored the Service , WiimoteInfo and Mainwindow . removed unnecessary pieces of code , project left in working state.

1.18.08.14 - First commit , so far small gui Draft is made and Wii remote can be connected to the app via the WiimoteStatus class.