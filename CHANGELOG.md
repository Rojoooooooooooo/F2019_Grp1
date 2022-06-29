# Changelog
> The source code is also available in the [source-code branch](https://github.com/Rojoooooooooooo/F2019_Grp1/tree/source-code). Standby for regular updates.

## As of 06/29/2022
> New path released!
**Major Update**
- Session handling reworked
**Atomic Update**
- Added mock layout for Pet Profile and Clinic Profile

## As of 06/26/2022
> ANNOUNCEMENT: Authentication will be reworked until tomorrow to fit the future development plans.
> Changes:
> - Session / Refresh token will be removed
> - Access Token will be used as session token using ASP.NET Identity.
> Tomorrow will be the release of the new patch.

## As of 06/25/2022

**Atomic Update** 
- Added more input validation on login, signup, and profile creation forms.
 
> **Note:** Those with **(x)** mark are currently specific for **WEB API only**. Standby for future developments.

## As of 06/24/2022

**Atomic Update** 
- Fixed JwtToken class returning the input token even it is null or invalid. The culprit is an absurd use of if-else statement.
- Fixed [Issue #1](https://github.com/Rojoooooooooooo/F2019_Grp1/issues/1#issue-1282520612)
- API error response for /auth/login and /auth/refresh refactored

> **Note:** Those with **(x)** mark are currently specific for **WEB API only**. Standby for future developments.


## As of 06/23/2022 [06/22/2022 commit](https://github.com/Rojoooooooooooo/F2019_Grp1/commit/dc7062b0803759998f7ed34c200d38d6845e4807) 

**Atomic Update** 
- Added pet creation on the client interface.
- Pet dashboard added includes:
   - Pet creation
   - Display owner's pet/s
- GET pet categories and breeds support in REST API(x).

> **Note:** Those with **(x)** mark are currently specific for **WEB API only**. Standby for future developments.

## As of 06/21/2022 

**Atomic Update** 
- Fixed error on JwtToken object returning blank string. This is caused by absurd implemntation of For-Each loop inside the JwtToken.GetPayload method.

> **Note:** Those with **(x)** mark are currently specific for **WEB API only**. Standby for future developments.

## As of 06/19/2022 

**New Features**
- Pet creation function added! (x)
- Fetch pet information (Id, Owner Id, Pet name) added! (x)

**Atomic Update** 
- Improved Owner Sign up input sanitization (x).

> **Note:** Those with **(x)** mark are currently specific for **WEB API only**. Standby for future developments.

## As of 06/18/2022

**The project "Pet Paradise" has reached its 10% of progress. Thus, the following features are supported:**

- Owner/Clinic Sign up.
- Owner/Clinic Add Basic Information
- Owner/Clinic Log in.
- Owner/Clinic Update Basic Information (x)
   - Name
   - Address
   - Contact Number
- Account authorization
- Account authentication (using JWT)

> **Note:** Those with **(x)** mark are currently specific for **WEB API only**. Standby for future developments.
