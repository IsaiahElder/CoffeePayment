
# Coffee Payments Management System

This C# console application manages coffee payments among employees. It tracks employees' usual orders, total payments made by each employee, and determines who should pay for the current order.

## Features

- **Order Management**: Employees can specify whether they are ordering their usual coffee or not.
- **Payment Calculation**: Calculates the total payment amount and updates the past total payments for the employee who is paying.
- **JSON Data Storage**: Utilizes JSON serialization to store and retrieve employee data.
- **Dynamic Employee Data**: If no data is available, the program generates initial employee data.

## Usage

1. Run the application.
2. Respond to the prompt whether everyone is ordering their usual coffee or not.
3. If everyone orders their usual, the program calculates the total payment based on stored data. Otherwise, it prompts for individual order prices.
4. After determining the payer, the program updates the total payments and stores the data in a JSON file.
5. You can choose to order again or exit the program.


## Build and Run Instructions

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) installed on your system.

### Build and Run Steps

1. **Open Terminal/Command Prompt**:
   - Navigate to the directory containing the source code files.
      ```
      cd <source file directory>
      ```
2. **Compile the Program**:
   - Run the following command to build the program:
     ```
     dotnet build
     ```

3. **Run the Program**:
   - Once the build is successful, execute the program using the following command:
     ```
     dotnet run
     ```
Alternatively, you can download the compilied executable from the releases section on Github.
### Usage Instructions

- When the program is executed, it will prompt you whether everyone ordered their usual coffee.
- Respond with either 'y' for yes or 'n' for no.
- If everyone ordered their usual, the program will calculate the total payment based on stored data.
- If not, it will prompt for individual order prices.
- After determining the payer, the program will update the total payments and store the data in a JSON file.
- You can choose to continue ordering or exit the program by responding to the "Would you like to order again?" prompt.

## Assumptions

- The program assumes that all seven people order coffee every day.
- The program uses placeholder names for the remaining 5 people.
