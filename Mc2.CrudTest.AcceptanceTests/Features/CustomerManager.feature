Feature: Customer Manager

As an operator I wish to be able to Create, Update, Delete customers and list all customers

Scenario: Create a new valid customer successfully
    Given I have the following customer information
        | FirstName | LastName | DateOfBirth | PhoneNumber   | Email              | BankAccountNumber |
        | John      | Doe      | 1990-01-01  | +12125551234 | john.doe@email.com | 123456789         |
    When I send a request to create the customer
    Then the customer should be created successfully
    And I should be able to retrieve the customer by ID

Scenario: Attempt to create duplicate customer
    Given I have an existing customer
        | FirstName | LastName | DateOfBirth | PhoneNumber   | Email              | BankAccountNumber |
        | John      | Doe      | 1990-01-01  | +1234567890  | john.doe@email.com | 123456789        |
    When I try to create another customer with the same details
    Then I should receive a duplicate customer error

Scenario: Update existing customer information
    Given I have an existing customer with ID
    When I update the customer with the following information
        | FirstName | LastName | DateOfBirth | PhoneNumber   | Email               | BankAccountNumber |
        | John      | Smith    | 1990-01-01  | +1987654321  | john.smith@email.com| 987654321        |
    Then the customer information should be updated successfully

Scenario: Delete existing customer
    Given I have an existing customer with ID
    When I send a request to delete the customer
    Then the customer should be deleted successfully
    And I should not be able to retrieve the deleted customer

Scenario: List all customers
    Given I have the following customers in the system
        | FirstName | LastName | Email               |
        | John      | Doe      | john.doe@email.com  |
        | Jane      | Smith    | jane.smith@email.com|
    When I request the list of all customers
    Then I should receive a list containing all customers

Scenario: Create customer with invalid email format
    Given I have the following customer information
        | FirstName | LastName | DateOfBirth | PhoneNumber   | Email        | BankAccountNumber |
        | John      | Doe      | 1990-01-01  | +1234567890  | invalid-email| 123456789        |
    When I send a request to create the customer
    Then I should receive an invalid email format error

Scenario: Create customer with invalid phone number
    Given I have the following customer information
        | FirstName | LastName | DateOfBirth | PhoneNumber | Email              | BankAccountNumber |
        | John      | Doe      | 1990-01-01  | 123456     | john.doe@email.com | 123456789        |
    When I send a request to create the customer
    Then I should receive an invalid phone number format error