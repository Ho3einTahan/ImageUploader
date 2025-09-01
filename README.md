### 📸 ImageUploader

#### A desktop application built with C# (Windows Forms) and MySQL for managing and storing images in a database.
#### This project is designed as an example of multimedia databases.

### ✨ Features

✅ Upload images from the user’s computer

✅ Store images in a MySQL database (as BLOB)

✅ Display a list of stored images in a DataGridView

✅ Preview image details by selecting from the list

✅ Simple and user-friendly interface

### 🛠️ Prerequisites

#### To run this project, you will need:

1. Windows 10 or higher

2. .NET Framework 4.7.2+

3. MySQL Server

4. MySql.Data library (already included in the project)

### ⚙️ Database Setup

#### Open MySQL and create a new database:

``` SQL
CREATE DATABASE Uploader;
```


#### Create the images table:

``` SQL
USE Uploader;

CREATE TABLE Image (
    ImageId INT AUTO_INCREMENT PRIMARY KEY,
    ImageName VARCHAR(255) NOT NULL,
    ImageData LONGBLOB NOT NULL
);
```


#### Update your connection string if necessary (in Form1.cs):

``` C#
private string connectionString = 
    "Server=localhost;Database=Uploader;Uid=root;Pwd=;SslMode=none;AllowPublicKeyRetrieval=True;";
```


### 👉 Change Uid and Pwd according to your MySQL credentials.

### 🚀 How to Run

1. Open the project in Visual Studio.

2. Right-click the solution and select Rebuild.

3. The output file Uploader.exe will be generated at:

4. bin/Debug/Uploader.exe


### Make sure the MySQL server is running.

### Run Uploader.exe.

### 📂 Project Structure
``` C#
ImageUploader/
│── Form1.cs          # Main form logic
│── Form1.Designer.cs # UI design
│── Program.cs        # Entry point
│── bin/              # Build output
│── obj/              # Temporary files
│── .gitignore        # Ignored files for Git
```

### 🔑 Key Code Snippets

#### Select an image:

``` C#
pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
```


#### Insert image into database:

``` SQL
string query = "INSERT INTO Image (ImageName, ImageData) VALUES (@ImageName, @ImageData)";
```


#### Display list of images:

``` SQL
string query = "SELECT ImageId, ImageName FROM Image";
dataGridView1.DataSource = table;
```


#### Preview selected image:

``` C#
byte[] imageData = (byte[])reader["ImageData"];
pictureBox1.Image = Image.FromStream(new MemoryStream(imageData));
```

#### 🙏 Thanks for using this tool!
#### © 2025 — All rights reserved. Developed with ❤️ by Ho3ein Tahan