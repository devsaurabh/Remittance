# Remitter API

This project contains API endpoints for sending money through a third party remittance provider.

## Libraries and Frameworks
* .Net Core 2.1
* Polly forresilience and transient-fault-handling
* Serilog for logging
* NUnit for testing
* Swagger for API browsing
* Dapper/EF (Proposed) for ORM needs

## Desgin Goals
* Resilient/Fallback mechanism when API provider failes 
* Idempotent endpoints (producing same output for same input)
* Modular design (Testable)
* Performance
 
### Resilient
When calling the external provider there are is always a possiblity of getting a failed result. This can be beacuse of network at our end or a problem at provider end. Such scenarios are un-avoidable. 

The idle scenario to implement this to have couple of providers on which we can do a round-robin (most probably keeping a score of service quality over failed and passed calls). Though to start with we have only one service provider, the current approach is
* **Retry Policy (Implemented)** - Retry the current provider for idempotent stuff (like getting the exchange rate and country list etc.). Also, there are couple of scenarios where it is better to retry for every call like when ServiceUnavailable (503)
* **Cache (Implemented)** - Cache the output of these endpoints and minimize the number of calls. This is also helpful in increasing the performance.
* **Circuit Breaker (Implemented)** - When the external provider is failing, it's most probably due to some problem which ususally is going to take some time to fix. Rather than keeping retrying the endpoint continuously, the circut break opens the circuit after fixed number of failures. It will then retry it after a fixed interval, if the call still fails, the circuit will remain broken or else, it will be closed.
* **Message Based (Proposed)** - There can be scenarios like when saving user transaction is submitted but the provider is down. In this case, since we already chanrged money from the custome, we can save this transaction on a local data store (DB) and also push this to a **Background Queue** or **RabbitMQ** which will keep retrying the provider for a success. This is also app

## Solution Design
The solution contains the following projects
* **Libraries/Remitter.Core** - Contains useful infrastructre which can be utilized at any layer like *Caching, Rest helper, Policies*
* **Libraries/Remitter.Data** - This layer contains all the infrastructure related to database repositories or calling the external providers. Since, the external provider is also a kind of datasource, it's ideal to place it at this level.
* **Libraries/Remitter.Business** - This the layer we have all the business logic. The logic is grouped based on the business like *Estimation, Payments, Tax and Fees etc.*
* **Tests/Remitter.Test** - This project contains the unit tests and intergrations tests (not implemented).
* **Web/Remitter.Api** - The Apis are implemented in .Net Core 2.1 Asp.Net in this project. **Swagger** is also added for testing the apis.

## APIs
| Endpoint | Verb | Description | Implemented | Authentication Required? |
| - | - | - | - | - |
| *api/country* | Get  | Gets the list of countries | Yes | No |
| *api/estimation/{estimationId}* | Get | Gets the already created estimation | Yes | No |
| *api/estimation* | Post | Create a new estimation | Yes | No |
| *api/beneficiary* | Get | Gets All beneficiaries | No | Yes |
| *api/beneficiary/{beneficiaryId}* | Get | Gets the selected beneficiary | No | Yes |
| *api/beneficiary* | Post | Creates a new beneficiary | No | Yes |
| *api/beneficiary/account* | Get | Gets all the mapped accounts | No | Yes |
| *api/beneficiary/account* | Post | Adds new account for user | No | Yes |
| *api/transaction | Post | Submit a transaction | No | Yes |
| *api/transaction/{id} | Get | Get transaction status | No | Yes |
| *api/user/login | Post | Login user | No | No |

## Design Flow
The intended flow is:
![alt text](https://raw.githubusercontent.com/devsaurabh/images/master/IMG_63DBB9155B0C-1.jpeg "Logo Title Text 1")
* User lands on the app and first selects the country and enters the amout to transfer. He can also change the Debit method (like credit card pay or direct debit) and also the transfer method like branch collect or account transfer (this is dependent upong the provider and country)
* The next screen is about Login (if not already done) so that the beneficiaries can be pulled from the data store. In this screen the user can also add a new beneficiary or add a account for the country of transfer.
* Next is all about making the payment to us using the selected payment method.
* Review is reviewing the summary of everything
* Post review, submit will submit the transaction where the user will be re-directed to the payment gateway for payment.

## Security and Authentication
The system is supposed to run on HTTPS. PCI is not-required as we are not saving any CC information. For authentication we can use token based authentication.

## User Registration
This has to be done online over the website and then later a visit by the user to nearby office. As this is related to KYC and document verificaiton.

## Error Handling and Logging
* **CustomMiddleware** - Added a custome middle ware to ASP.Net pipeline which will catch and log exceptions. This is also useful in returning a more meaning full response to the API consumer.
* **Logging** - **Serilog** logging middle ware is used with is configured to write a Rotation file logs on the disk
