# Appointment Booking Basic

Basic Multi-Tenant appointment booking system created to play with MVC, Authentication, Patterns and Entity Framework.

It's nowehere near complete and a bit hacky in parts, but there's also plenty of good stuff in there!

I used .NET full instead of .NET Core as when I started .NET Core was at 1.1 when I started and the .NET Core version of Entity Framework didn't fit my needs (you couldn't make MVC models from EF in another project easily).

If I was making it now, I'd use .NET Core. The processes of getting data, using patterns and presenting to a UI would be very similar.


## Features

- Responsive (works on all sizes from phone to desktop).
- Authentication (tables moved to main DB to save money on hosting, but could be easily split out again)
- Create a user (professional)
- Add company details for that user
- Invite employees to that company (they may take appointments or not).

- Employees can set availability
- Employees can book appointments for themselves
- Company owners can book appointments for employees.

- Load tested with 1 million companies and over 1 million professionals.
- No unit tests. For examples of my unit testing, see my [UK Accident Statistics](https://github.com/HockeyJustin/UkAccidentStatistics/tree/master/src/AccidentProcessor.Tests) project.

# To Run

- Clone Repro
- Open in VS2017. You will also need MS SQL Server installed on your machine.
- Change the database urls in `AppointmentsMVC` (web.config) and `AppointmentsDb` (app.config) projects.
- Ensure 'AppointmentsMVC' is the startup project and hit Run.
- If there's a problem, open the package manager console. It will state packages are missing. Click 'restore' to install them. Run again.

When the site starts:

- Click 'Register' in the top right hand corner to register a new user (single factor auth, so you can add anything in the email field).

![Register new user](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/tree/master/Screenshots/1Register.PNG?raw=true)

![alt tag](https://github.com/HockeyJustin/UkAccidentStatistics/blob/master/src/AccidentProcessor/Resources/Reference/_area_Newbury_To_Maidenhead_A339_A34_M4_A308M.png?raw=true)

- Enter Profile

![Profile](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/tree/master/Screenshots/Screenshots/2 Profile.PNG?raw=true "Profile")

- Enter Company Info

![Company Info](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/tree/master/Screenshots/Screenshots/3 Company.PNG?raw=true "Company Info")

- Click on a date to enter an appointment and add one.

![Appointments View](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/tree/master/Screenshots/Screenshots/4 Appointments For Self?raw=true "Appointments View")

![Create Appointment](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/tree/master/Screenshots/Screenshots/5 Appointments For Self 2.PNG?raw=true "Create Appointment")

- Can now see the appointment in the list

![Appointment](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/tree/master/Screenshots/Screenshots/6 Appointments For Self 3.PNG?raw=true "Appointment")

- Also looks fine on iPhone.

![iPhone View](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/tree/master/Screenshots/Screenshots/6 Appointments For Self 3 - iPhone view.PNG?raw=true "iPhone View")

- Go to Admin dropdown and select - Company Locations.

![Admin Menu](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/tree/master/Screenshots/Screenshots/7 Admin Dropdown.PNG?raw=true "Admin Menu")

- Enter a location or set of locations and save (these aren't actually used anywhere in the system).

![Company Locations](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/tree/master/Screenshots/Screenshots/8 Company Locations.PNG?raw=true "Company Locations")

## To Add employees
- Go to the Admin dropdown and select 'Keys'. This will enable you to invite users.
- Click Add a professional to company.

![Add employee step 1](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/tree/master/Screenshots/Screenshots/9 Add Pro to Company 1.PNG?raw=true "Add employee step 1")
![Add employee step 2](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/tree/master/Screenshots/Screenshots/9 Add Pro to Company 2.PNG?raw=true "Add employee step 2")


- Copy the URL.

![Add employee step 3](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/tree/master/Screenshots/Screenshots/9 Add Pro to Company 3.PNG?raw=true "Add employee step 3")

- LOG OUT!!!
- Now paste the URL into the browser. 
- Select 'Yes'.

![Join company](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/tree/master/Screenshots/Screenshots/9 Add Pro to Company 4.PNG?raw=true "Join company")

- Register as a new user (yes, that should be improved!)

![Join company](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/tree/master/Screenshots/Screenshots/9 Add Pro to Company 5.PNG?raw=true "Join company")

- Enter professional details

![Employee Details](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/tree/master/Screenshots/Screenshots/9 Add Pro to Company 6.PNG?raw=true "Employee Details")

- It knows your are in that company. They can now create appointments for themself.

![Employee Created](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/tree/master/Screenshots/Screenshots/9 Add Pro to Company 7.PNG?raw=true "Employee Created")

- Click 'Appointments' and create an appointment as the employee for the ownwer to see.

![Employee Appointment Booking](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/tree/master/Screenshots/Screenshots/9 Add Pro to Company 8.PNG?raw=true "Employee Appointment Booking")

## Company owner accessing employees' diary.

- Log out and log in as the owner.
- You can now see the employees' diary.

![Owner Access Appointments](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/tree/master/Screenshots/Screenshots/10 Owner accesses employee appointments 1.PNG?raw=true "Owner Access Appointments")

![Owner Access Appointments 2](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/tree/master/Screenshots/Screenshots/10 Owner accesses employee appointments 2.PNG?raw=true "Owner Access Appointments 2")

- You can now add appointments for the employee.

![Owner Creating appointment for employee](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/tree/master/Screenshots/Screenshots/10 Owner creating appointment for employee.PNG?raw=true "Owner Creating appointment for employee")






