# Introduction 
The project LoansComparer is meant to be an online loan offers' comparing website. We think such a tool where a user can input their data only once and get all credible offers, instead of visiting
every single bank's website and copy-paste the same data, is incredibly useful and makes life (and the whole process of taking a loan) much less stressful and much more convenient.
In our app, Users can make an inquiry, both after registering at our website - so as a logged in user, and as an unregistered user. Then they can check, what offers they received (they get offers from
3 different banks) - and choose one of them. Then they submit the agreement and the whole process is completed. The LoansComparer project contains calls to 3 different bank API's for getting 3 offers 
for each inquiry.It also contains a website for a bank employee, where bank employee can login and accept or decline the offer requests from clients.Bank employee can also see the history etc.

# Getting Started
1.	Installation process
    To install the app, you need to clone the repository to your local computer. Then you need to configure your SendGrid account and Gmail auth service. You should also check if the connection to the
    database and Azure Blob Storage is working correctly.
2.	Software dependencies
    The project uses the following NuGet packages: cloudscribe.Pagination.Models, cloudscribe.Web.Pagination, IdentityModel, IdentityModel.AspNetCore, Microsoft.AspNetCore.Authentication.Google, 
    Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore, Microsoft.AspNetCOre.Identitiy.EntityFrameworkCore, Microsoft.AspNetCore.Identity.UI, Microsoft.EntityFrameworkCore, Microsoft.EntityFrameworkCore.Design,
    Microsoft.EntityFrameworkCore.SqlServer, Microsoft.EntityFrameworkCore.Tools, Microsoft.IdentityModel, Microsoft.VisualStudio.Web.CodeGeneration.Design, RestSharp, SendGrid, System.Text.Json. 
3.	API references
    The project references and connects to 3 different bank's APIs:
    https://mini.loanbank.api.snet.com.pl/swagger/index.html
    https://best-bank-webapi.azurewebsites.net/swagger/index.html
    https://fictionbank20230111105722.azurewebsites.net/swagger/index.html
4. Latest releases
    The current and up-to-date version of the project is the one on main branch.

# Build and Test
To build the code, follow the Installation process. The tests to frontend side of the project are present in the SeleniumTests repository. Tu run them, you should clone both this project and that repository
to your local computer. Then run this app and, while it is running, start the Selenium tests.

# Contribute
To contribute, feel free to write to us for access to pushing code to this repository. Then simply create a branch, checking out from main and do your code changes there. Finally, after we've accepted your code,
merge your branch to main branch. That's it :).

# Trunk based development
While working on this project, we were using the trunk based development flow. main has been our 'trunk' branch. Following the trunk based development rules, to complete any tasks, each of us was checking out
from main to her own branch, then making changes on that branch. Then we were testing whether our changes do not disturb any other working project functionalities. If all the tests have been passed, we were then
merging the code with the main branch (after the code has been reviewed and accepted by the rest of the team, of course). This approach, hugely popular nowadays, allowed us to have continous integration of code. 
We found small, often (daily/hourly) changes as easier and better ways of coding than making huge merges once a week and then spending the whole day solving merge conflicts.