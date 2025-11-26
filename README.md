# ProcurementFlowMVC: Bridging Code and Commerce

This isn't just another project; it's a deep dive into the engine room of enterprise commerce! **ProcurementFlowMVC** is a web application built using **ASP.NET Core MVC** that brings the powerful, structured workflows of systems like **Oracle Fusion Procurement** right to your desktop.

Goal was simple: to create a robust, yet focused, system that perfectly simulates the journey of a purchase from request to order.

We've concentrated on the core logic that makes procurement work, making the code clean and the workflow clear.

* **Real-World Workflow:** We model the essential life cycle: **Purchase Requisition (PR)** creation leads directly to **Purchase Order (PO)** generation. This is the heart of any reliable procurement system.
* **The Manager Gatekeeper:** Governance is key! We include critical **Approval and Rejection** steps, ensuring every purchase request gets the necessary sign-off from a manager before it proceeds.
* **Beyond the Catalog:** Need a custom item? Our system supports adding **Non-Catalog Items**, ensuring flexibility for all organizational needs.
* **Central Command Console:** The **Transaction Console** acts as your central control tower. It's the one place to track the real-time status and complete history of *every single requisition*.
* **Actionable Insights:** Two types of dashboards provide instant clarity: one for the **Overall Organization** (what's the big picture?) and another tailored for the **Individual Manager** (what needs my attention right now?).

## The Data Engine: SQL Stored Procedures

We didn't just use SQL Server; we used it strategically! All data handling, retrieval, and manipulation—everything that moves the procurement process forward—is handled by **highly optimized Stored Procedures (SPs)**. This ensures:

* **Performance:** Faster data access and transaction execution.
* **Security:** Logic is hidden on the database server, enhancing data integrity.
* **Scalability:** A clean separation of application logic (C#) from data logic (SQL Server SPs).

## Tech Stack & Getting Involved

This project is built on reliable Microsoft foundations:

| Component | Focus |
| :--- | :--- |
| **Backend** | **C#** and **ASP.NET Core MVC** |
| **Database** | **SQL Server** (Heavy use of Stored Procedures) |
| **Architecture** | Clear **MVC** structure |
