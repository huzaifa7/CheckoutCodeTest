# CHeckout Code Test 

Contents
========

[1: Overview](#1-overview)
[2: Setup](#2-setup)
[3: Usage](#3-usage)
[4: Assumptions](#4-assumptions)
[5: Improvements](#5-improvements)


1: Overview
=============================================

Build a payment gateway and simulate the acquiring bank component in order to allow us to fully test the payment flow

## Components 

-   Payment Gateway Api - merchants will be interacting with this api

-   Bank Api - stubbed api to simulate interaction with the acquiring bank


2: Setup
=============================================

The solution includes a set of component tests that verify the behavior of the payment gateway api.
In order to run these tests: 
1.  Press `F5` to start up the PaymentGatewayApi and the BankApiStub.

2.  Run the following command from the solution directory: `dotnet test .\PaymentGatewayApi.ComponentTests\.`



3: Usage
=============================================

The Payment Gateway Api have the following features:
-   process a payment through the payment gateway and receive either a successful or unsuccessful response
-   ability to retrieve the payment details of a previously made payment using a payment reference

## Process a payment request and receive a response

### PaymentRequest Model

```
public PaymentRequest(string cardNumber, string expiryDate, decimal amount, string currency, int cvv)
{
    CardNumber = cardNumber;
    ExpiryDate = expiryDate;
    Amount = amount;
    Currency = currency;
    Cvv = cvv;
}
```

### PaymentResponse Model

```
public PaymentResponse(Guid paymentReference, string status)
{
    PaymentReference = paymentReference;
    Status = status;
}
```

## Retrieve payment details using a payment reference

### PaymentReference Model

```
Guid paymentReference
```

### PaymentDetail Model

```
public PaymentDetail(Guid paymentReference, string status, string cardNumber, string expiryDate, decimal amount, string currency, int cvv)
{
    PaymentReference = paymentReference;
    Status = status;
    CardNumber = cardNumber;
    ExpiryDate = expiryDate;
    Amount = amount;
    Currency = currency;
    Cvv = cvv;
}
```


4: Assumptions
=============================================
The Payment Gateway Api is returning a OK response for both successful and unsuccessful statuses from the bank api stub.
The reason for this is that the the payment gateway service has managed to process the request successfully, it's just the outcome of the request was either *ACCEPTED* or *DECLINED*


5: Improvments
=============================================

Here are the following suggestions for improvemnts:

-   Validate paymentRequest against business rules [PaymentGatewayService.cs]

-   Use cache-aside pattern to retrieve payment details from storage to improve performance and save cost [PaymentGatewayService.cs]

-   Enrich logger to capture context (i.e correlation id) [PaymentGatewayService.cs]

-   Create a dockerfile for the PaymentGatewayApi, BankApiStub & PaymentGatewayApi.ComponentTests project and use docker-compose to spin-up the 2 apis before running the tests. This setup would enable these tests to run in the CI pipeline