# ContactApp

### Installation

## 1. Clone the repo
   ```sh
   git clone https://github.com/adinovcina/ContactApp.git
   ```

## 2. Change connection string in appsettings.json for:
	-ContactApp.Api 
	-ContactApp.IdentityServer

## 3. Select multiple startup project:
	- ContactApp.Api
	- ContactApp.IdentityServer
	- ContactApp.UI
	
## 4. Double check ports for localhost in appsettings.json for all projects



<!-- Project arhitecture -->
## Project arhitecture:

- 📂 Api
- 📂 IdentityServer
- 📂 UI
- 📂 Bootstrapper
- 📂 Services
- 📂 Models
- 📂 Data.EF
- 📂 Api.Tests
- 📂 Repository.Tests



### Potential improvements:
- Implement error handling for backend exceptions in the frontend
- Design an address table with defined properties
- Implement logging on the backend for better tracking and debugging
- Enhance frontend validation to improve user experience
- Improve the user interface design for a more visually appealing experience
