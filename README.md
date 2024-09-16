<div align="center">

 <img src="https://github.com/pietraldo/Airport-Management-System/blob/main/Zrzut%20ekranu%202024-09-13%20182230.png" width="60%" />
</div>

# Airport Management System

Developed incrementally, this Airport Management System simulates real-time flights and their management. It supports reading and serializing data from FTR files, handling real-time TCP data streams, snapshot creation, object updates, and data filtering. The architecture emphasizes modularity for ease of extension and maintainability.

## ✏️ Console Commands
Execute these commands within the console:

- `print` – Takes a snapshot of the entire dataset and saves it.
- `report` – Displays current data on the console.
- `exit` – Closes the application.

## 📝 Custom SQL-like Data Management
Execute SQL-like commands to manage data:

- `display` – Formats and displays data as a table.
  - Syntax: `display {object_fields} from {object_class} [where {conditions}]`
  
- `update` – Modifies existing data.
  - Syntax: `update {object_class} set {key_value_list} [where {conditions}]`
  
- `delete` – Removes data entries.
  - Syntax: `delete {object_class} [where {conditions}]`
  
- `add` – Introduces new data entries.
  - Syntax: `add {object_class} new {key_value_list}`

## 📄 Logging
Logs capture all data alterations, including updates, ID changes, and failed operations. Sample logs:
```text
Id: 1329, ID changed from 499 to 1329
Operation UpdateId on object 1475 failed
Id: 1073, Position changed from (157.99564, 3.403886, 11982) to (-145.21829, -87.924355, 292.1136)
