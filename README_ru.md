<h3 align="center">ExcelShopSync</h3>

<div align="center">
    <a href="./LICENSE"><img src="https://img.shields.io/github/license/FreidFry/ExcelShopSync?label=License&color=brightgreen&cacheSeconds=3600" alt="License"/></a>
    <a href="http://github.com/FreidFry/ExcelShopSync/releases/latest"><img src="https://img.shields.io/github/v/release/FreidFry/ExcelShopSync?style=square&color=brightgreen&cacheSeconds=3600" alt="Release"/></a>
</div>

#### Другие языки
- [Українська](README_ua.md)
- [Русский](README_ru.md)

This program is designed to simplify and partially automate the process of updating price lists and product availability on marketplaces and in online stores. It helps to quickly and efficiently synchronize data between different sources, minimizing manual labor and reducing the likelihood of human errors.

### The provided functionality allows:
- Transferring by article numbers:
    - Product prices
    - Product availability and quantity
    - Discounts (with dates and without)
    - Product statuses (e.g., "Ready to ship")
- Increasing prices by a certain percentage.
- Specifying links for different article numbers to a product.
- Working with multiple files simultaneously.

---

## Program Usage Instructions

The program is intended for processing data from Excel files — synchronizing prices, availability, discounts, and other parameters between price lists and marketplaces.

⚠️ **Important:** The program works **by article numbers**, exact match is mandatory unless a link is specified. Even a difference in one character (e.g., `K` and `К`) is critical.

---

## Preface:
- It is recommended to create backup copies of files before processing.
- The program does not support macros and complex formulas in Excel files.
- Processing large files may take some time, please be patient.
- The program is not intended for processing password-protected files.
- If you encounter problems or have questions, please contact the details at the end of the instructions.

### Recommendations before starting

- Follow the instructions below.
- All Excel files must be **closed** before starting processing.
- Use files in the formats: `.xlsx`, `.xls`.
- If errors occur, check the correctness of the headers and the presence of all necessary columns.

---

#### Description of subsequent terms:
- **Target** — the file(s) where changes will be made.
- **Source** — the file(s) from which data for changes will be taken.
- **Article Number** — a unique product identifier used for matching between Target and Source.
- **Price List** — the Source file containing current product prices.

- **Required fields** — columns that must be present in the files for the program to work correctly.
- **Additional fields** — columns that can be used for extended functionality but are not mandatory.

---

## Usage

<details>
<summary>Main Window</summary>

### 1. Loading Target Files
- Button: **Add File for Changes**
- Used for making changes.

### 2. Loading Source Files
- Button: **Add Source File**
- Used as data sources.

> **Recommendation:** use a minimal number of Source files with duplicate article numbers to avoid conflicts.
> - **Reason:** if the same article number appears in several Source files, the value from the first found product with this article number will be used.

> **Tip:** if you have several price lists **WITH THE SAME PRODUCTS**, merge them into one file before processing to get the expected result.
> - If you have no matching article numbers between files, then you can use multiple Source files.

---

## Operation Selection

### Price Transfer
- **Checkbox:** `Transfer Prices`
#### Required Fields:
- `Article`, `Price`
- Additionally: `ArticleComplect`, `PriceComplect` (in the price list)

> **Mandatory:** if you are transferring values from a price list, the values must be **under the corresponding columns**.

---
### Availability Transfer
- **Checkbox:** `Transfer Availability`
#### Required Fields:
- `Availability`
- Additionally: `Availability Complect` (in the price list)

---

### Price Increase
- **Checkbox:** `Increase Price (Pseudo Discount)`
- Specify the markup percentage within the range from `1` to `200`.

> **Details:** Treated as a percentage. 1 == 101%, 2 == 102%, 99 == 199%, 100 == 100%, 101 == 101%... 200 == 200%, 201 == 201%...  
> entering `0` or `100` - the price will not change.

> **Hint:** if you want to increase the price by 10%, enter `10` or `110`. But if you enter more than 200, you will be asked for confirmation.

---

### Discount Transfer
- **Checkbox:** `Transfer Discount`
#### Required Fields:
- `Discount` (only from store to store)

> **Important:** the transfer of discount percentages works in such a way that it **does not convert 110% to 10%**, but simply **copies the value from one column to another.** Be careful when using.

---

### Discount Date Transfer
- **Checkbox:** `Transfer Discount Date`
#### Additional Fields:
- `discount from/to` (only from store to store)

> **Mandatory:** The store settings must specify the `Date Format` column.  
> `Discount End Date` and `Discount Start Date` are used only if they are present in the store.  
> There is actually no such field as "Discount Date". The date is taken from a column in one store and transferred to a column in another store if it is present.

---

### Finding Missing Article Numbers
- **Checkbox:** `Find Missing`
- Allows finding article numbers that are in the Target files but are missing in the Source files.
- The result is saved in the `missing.txt` file on the desktop.

---

### Launch
- Button: **Start**
1. Make sure files are loaded
2. Set parameters
3. Click "Start"
4. Wait for the `Done` message

---

## Mandatory Conditions

- Column names must be correct (If it's a price list):
    - **Article Number:** `Article`, `Артикул`, `ArticleComplect`
    - **Price:** `Price`, `Ціна`, `Price Complect`
    - **Availability:** `Availability`, `Наличие`, `Availability Complect`

❗All headers must be written **exactly**, without extra characters, spaces, or errors.

</details>

<details>
<summary> Store Manager </summary>

### Opening the Store Manager
- `Settings` -> `Store Manager`

### Window Description
- **Left:** list of stores with the ability to select for editing. (Unnecessary stores can be deleted)
- **Button bottom-left:** Allows adding stores.
- **Right:** list of available columns from the loaded file.
- **Center:** Fields for column names corresponding to various parameters (values are taken from the column on the right).

> **Mandatory:** since the program uses the list on the right (All Columns) to identify the store - leave only those columns that are in your file and are ALWAYS IN EVERY FILE.  
> Columns that depend on the product category (e.g., `color`, `size`, `material`, `material|45234`, etc.) - will interfere with normal operation, so they need to be removed from the list.

### Purpose
- Allows saving column settings for various stores.
- You can add, delete, and edit stores.

> **Please note:** When saving, **changes will be available on the next program launch.**

### Adding a Store
- Button: `Add Store`
- Enter the store name.
- Click the `Get Store Headers` button
- Specify the column names for the article number, price, availability, and other parameters present in the store.

</details>

<details>
<summary>Product Link Table</summary>

### Opening the Product Link Table
- `Product Link Table`

### Window Description
- **Button: `Add New Product`**
- Table with columns:
> **Master** - the main product article number to which other article numbers will be linked. **(article number from the price list)**  
> **Linked Article** - the article number that will be linked to the main one. **(article number from the store)** The column name is the store where this article number is used.

> **Attention:** if you have multiple stores, then for each store there should be its own column with the store name.  
> If your article number in the store matches the article number in the price list - you do not need to add a link.
> If your article number in a specific store matches the article number in the price list - add links only for those stores where the article number differs. (but you can add for all stores if you wish)

</details>

---

## Examples

### Example 1

✅ Correct View:

| Article | Price | Article Complect | Price Complect |
 |---------|-------|------------------|----------------|
| A001    | 100   | A001-C           | 80             |

❌ Error:

| Артикул | Article | Цена |
 |---------|---------|------|
| A001    |         | 100  |

> In this case, only the `Article` column will be processed. And since it has no value
>     - the product will not be found and processed.

### Example 2

✅ Correct View:

| Article  | Price    | Availability | any | any   | any |
 |----------|----------|--------------|-----|-------|-----|
| Product1 | 200      | +            | ... | ...   | ... |
| any      | Article  | Availability | any | Price | any |
| ...      | product2 | +            | ... | 300   | ... |
| ...      | product3 | +            | ... | 700   | ... |

❌ Error:

| Article  | Price    | Availability | any | any   | any |
 |----------|----------|--------------|-----|-------|-----|
| Product1 | 200      | +            | ... | ...   | ... |
| any      | Article  | Availability | any | Price | any |
| ...      | product2 | +            | ... | 300   | ... |
| product3 | 700      | +            | ... | ...   | ... |

> The output will be -> Product1 with price 200 in stock, Product2 with price 300 in stock, Product3 - ignored

### More details in the instruction located in the application.
`More` -> `Instruction`

---

## Reiteration of Important Points

<details>
<summary>General</summary>

> - Double-check the results after processing.

> - Follow the instructions below.

> - All Excel files must be **closed** before starting processing.

> - Use files in the formats: `.xlsx`, `.xls`.

> - If errors occur, check the correctness of the headers and the presence of all necessary columns.

> - The program works **by article numbers**, exact match is mandatory unless a link is specified. Even a difference in one character (e.g., `K` and `К`) is critical.

> - It is recommended to create backup copies of files before processing.

> - The program does not support macros and complex formulas in Excel files.

> - Processing large files may take some time, please be patient.

</details>

<details>
<summary> Operations </summary>

> **Recommendation:** use a minimal number of Source files with duplicate article numbers to avoid conflicts.
> - **Reason:** if the same article number appears in several Source files, the value from the first found product with this article number will be used.

> **Tip:** if you have several price lists **WITH THE SAME PRODUCTS**, merge them into one file before processing to get the expected result.
> - If you have no matching article numbers between files, then you can use multiple Source files.

> **Mandatory:** if you are transferring values from a price list, the values must be **under the corresponding columns**. (relevant for price lists, stores usually adhere to the standard anyway)

### Price Increase
> **Details:** Treated as a percentage. 1 == 101%, 2 == 102%, 99 == 199%, 100 == 100%, 101 == 101%... 200 == 200%, 201 == 201%...  
> entering `0` or `100` - the price will not change.

> **Hint:** if you want to increase the price by 10%, enter `10` or `110`. But if you enter more than 200, you will be asked for confirmation.

### Discount Transfer

> **Important:** the transfer of discount percentages works in such a way that it **does not convert 110% to 10%**, but simply **copies the value from one column to another.** Be careful when using.

### Discount Date Transfer
> **Mandatory:** The store settings must specify the `Date Format` column.  
> `Discount End Date` and `Discount Start Date` are used only if they are present in the store.  
> There is actually no such field as "Discount Date". The date is taken from a column in one store and transferred to a column in another store if it is present.

</details>

<details>
<summary>Store Manager</summary>

> **Mandatory:** since the program uses the list on the right (All Columns) to identify the store - leave only those columns that are in your file and are ALWAYS IN EVERY FILE.  
> Columns that depend on the product category (e.g., `color`, `size`, `material`, `material|45234`, etc.) - will interfere with normal operation, so they need to be removed from the list.

</details>

<details>
<summary>Product Link Table</summary>

> **Attention:** if you have multiple stores, then for each store there should be its own column with the store name.  
> If your article number in the store matches the article number in the price list - you do not need to add a link.
> If your article number in a specific store matches the article number in the price list - add links only for those stores where the article number differs. (but you can add for all stores if you wish)

</details>

---

## Contacts

Questions/Suggestions?  
Write to:  
[Telegram](https://t.me/Freid4)  
[Email](mailto:f4labs.study@gmail.com)

---

# Disclaimer:
Use of this program is at your own risk. The author is not responsible for any losses or damages arising from the use of the program. Any errors or problems encountered during the use of the program cannot be presented to the author as claims or demands for compensation. Furthermore, the author does not guarantee that the program will work without failures or errors and is not responsible for any consequences arising from the use of the program. Errors in the program may be fixed in future versions, but the author is not obligated to provide support or updates for the program. Users must independently assess the risks and take precautions when using the program. Any errors related to incorrect results are the result of your inattention and cannot be used against the author.