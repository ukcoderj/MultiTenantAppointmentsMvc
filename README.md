# Basic MVC Appointment Booking Application

Basic Multi-Tenant appointment booking system created to play with MVC, Authentication, Patterns and Entity Framework.

It's nowehere near complete and a bit hacky in parts, but there's also plenty of good stuff in there!

Uses .NET instead of .NET Core. When I started .NET Core was at 1.1 and the related version of Entity Framework didn't fit my needs (you couldn't make MVC models from EF in another project easily).

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

If there's a problem, open the package manager console. It will state packages are missing. Click 'restore' to install them. Run again.

When the site starts:

- Click 'Register' in the top right hand corner to register a new user (single factor auth, so you can add anything in the email field).

![Register new user](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/blob/master/Screenshots/1%20Register.PNG?raw=true)

- Enter Profile

![Profile](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/blob/master/Screenshots/2%20Profile.PNG "Profile")

- Enter Company Info

![Company Info](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/blob/master/Screenshots/3%20Company.PNG "Company Info")

- Click on a date to enter an appointment and add one.

![Appointments View](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/blob/master/Screenshots/4%20Appointments%20For%20Self.PNG?raw=true "Appointments View")

![Create Appointment](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/blob/master/Screenshots/5%20Appointments%20For%20Self%202.PNG?raw=true "Create Appointment")

- Can now see the appointment in the list

![Appointment](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/blob/master/Screenshots/6%20Appointments%20For%20Self%203.PNG?raw=true "Appointment")

- Also looks fine on iPhone.

![iPhone View](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/blob/master/Screenshots/6%20Appointments%20For%20Self%203%20-%20iPhone%20view.PNG?raw=true "iPhone View")

- Go to Admin dropdown and select - Company Locations.

![Admin Menu](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/blob/master/Screenshots/7%20Admin%20Dropdown.PNG?raw=true "Admin Menu")

- Enter a location or set of locations and save (these aren't actually used anywhere in the system).

![Company Locations](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/blob/master/Screenshots/8%20Company%20Locations.PNG?raw=true "Company Locations")

## To Add employees
- Go to the Admin dropdown and select 'Keys'. This will enable you to invite users.
- Click Add a professional to company.

![Add employee step 1](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/blob/master/Screenshots/9%20Add%20Pro%20to%20Company%201.PNG?raw=true "Add employee step 1")
![Add employee step 2](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/blob/master/Screenshots/9%20Add%20Pro%20to%20Company%202.PNG?raw=true "Add employee step 2")


- Copy the URL.

![Add employee step 3](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/blob/master/Screenshots/9%20Add%20Pro%20to%20Company%203.PNG?raw=true "Add employee step 3")

- LOG OUT!!!
- Now paste the URL into the browser. 
- Select 'Yes'.

![Join company](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/blob/master/Screenshots/9%20Add%20Pro%20to%20Company%204.PNG?raw=true "Join company")

- Register as a new user (yes, this should be improved!)

![Join company](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/blob/master/Screenshots/9%20Add%20Pro%20to%20Company%205.PNG?raw=true "Join company")

- Enter professional details

![Employee Details](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/blob/master/Screenshots/9%20Add%20Pro%20to%20Company%206.PNG?raw=true "Employee Details")

- It knows your are in that company. They can now create appointments for themself.

![Employee Created](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/blob/master/Screenshots/9%20Add%20Pro%20to%20Company%207.PNG?raw=true "Employee Created")

- Click 'Appointments' and create an appointment as the employee for the ownwer to see.

![Employee Appointment Booking](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/blob/master/Screenshots/9%20Add%20Pro%20to%20Company%208.PNG?raw=true "Employee Appointment Booking")

## Company owner accessing employees' diary.

- Log out and log in as the owner.
- You can now see the employees' diary.

![Owner Access Appointments](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/blob/master/Screenshots/10%20Owner%20accesses%20employee%20appointments%201.PNG?raw=true "Owner Access Appointments")

![Owner Access Appointments 2](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/blob/master/Screenshots/10%20Owner%20accesses%20employee%20appointments%202.PNG?raw=true "Owner Access Appointments 2")

- You can now add appointments for the employee.

![Owner Creating appointment for employee](https://github.com/HockeyJustin/MultiTenantAppointmentsMvc/blob/master/Screenshots/10%20Owner%20creating%20appointment%20for%20employee.PNG?raw=true "Owner Creating appointment for employee")






