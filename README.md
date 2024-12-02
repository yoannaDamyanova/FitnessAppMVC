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

Disclaimer: To become instructor a user has to provide a "legitimate" License number. License numbers are generated using a background service and are used just for showcasing (they wont regenerate if they already exist). All license numbers can be found in the FitnessApp.Services.Data folder in the subfolder Licenses.
