FitnessClassBooking System is a simple ASP.Net MVC project.
Functunalities:
1. User -> 
   - Book/Cancel Fitness class bookings
   - Review Finished classes
   - Give Instructors ratings
   - Become Instructor
2. Instructor
   - Add/Edit/Delete Fitness classes
   - Cancel classes
3. Admin
   - Add/Edit/Delete/Cancel/Approve Fitness classes
4. FitnessClass
   - Each class can acquire for different statuses(active, canceled, finished, full). On every load the StartTime of each class is checked for being in the past --> if the StartTime is a past date, the status becomes           finished. 

Disclaimer: To become instructor a user has to provide a "legitimate" License number. License numbers are generated using a background service and are used just for showcasing (they wont regenerate if they already exist). All license numbers can be found in the FitnessApp.Services.Data folder in the subfolder Licenses.

Credentials:
Admin:
   - email: admin@gmail.com
   - password: admin123
Instructor:
   - email: instructor@gmail.com
   - password: john123

Instructions for loading:
The application's startup project should be FitnessApp.Web.
Then you shoudl type update-database in the Package Manager Console, ensuring that the Default Project is Data\FitnessApp.Data.
The connection string constant is in the appsettings.json file.
