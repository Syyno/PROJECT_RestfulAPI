# PROJECT_RestfulAPI

This project represents an Restful API written in *ASP.NET Core 3.1*

## **Key features:**

- Data shaping
- Content negotiantion
- Searching data 
- Sorting data by field (ascending or descending)
- HATEOAS support

## **Technologies used:**

- *Platform:* ASP.NET Core 3.1 
- *Data access:* Entity Framework Core 3.1

## How to use

- On first use, you can request root **(/api)** to get links that can guide you to the next step.</br>

- Outside of root, *HATEOAS* disabled by default. To enable it, you need to include **Accept** header to your request with value of **"application/vnd.my.hateoas+json"**.</br>

- *Data shaping* can be used via *query string*, with help of *fields* param.  Example: **fields=id,name,price**</br>

- *Searching* can be done via model properties, or via  **searchQuery** param. Example: **category=pc** - searching via model property name </br>or **searchQuery=pc** - searching via searchQuery param </br>

- To sort data, use **orderBy**  param equals field name, base on which sorting will be implemented. By default result sorted in ascending order.<br/>
For descending order, add *"desc"* after field name, seperated by comma.</br>
Example: **orderBy=price** - ordering by field "price" in ascending order. **orderBy=price,desc** - ordering by field "price" in descending order.
