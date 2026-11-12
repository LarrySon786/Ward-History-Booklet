# Ward History Booklet Generator

A C#/.NET application that transforms structured Google Form submissions into a formatted PDF history booklet.

This project was created to help preserve the stories, experiences, and memories of individuals and families in a local church congregation. Rather than manually designing a booklet for each submission, the application automates much of the process, including data processing, photograph retrieval, page layout, and PDF generation.

## 📖 Project Overview

The Ward History Booklet Generator collects information through Google Forms, processes the submitted data, and generates a multi-page PDF booklet.

Participants can contribute family histories, missionary experiences, conversion stories, photographs, and other memories. Most fields are optional, allowing individuals to choose how much information they wish to share.

The application dynamically adjusts its output based on the information provided, producing consistent page layouts despite differences in submission lengths and available photographs.

## ✨ Features

- **Automated PDF Generation:** Converts structured submission data into a formatted, printable PDF document.
- **Google Forms Integration:** Processes Google Forms responses exported through Google Sheets as CSV files.
- **Google Drive Integration:** Uses Google's Drive API and OAuth 2.0 to retrieve submitted photographs.
- **Image Caching:** Stores downloaded photographs locally to avoid unnecessary repeated downloads.
- **Dynamic Page Generation:** Creates different page layouts depending on which optional fields contain information.
- **Adaptive Text Sizing:** Adjusts font sizes to accommodate varying text lengths within fixed page dimensions.
- **Photograph Formatting:** Supports portrait and landscape photographs while preserving their aspect ratios.
- **Custom HTML/CSS Templates:** Uses reusable HTML and CSS layouts to generate consistently styled pages.
- **Multiple History Categories:** Supports family histories, missionary experiences, and conversion stories.
- **Print-Oriented Layout:** Uses standard US Letter page dimensions with consistent headers, footers, and page numbering.

## 🛠️ Technologies Used

| Technology | Purpose |
|---|---|
| C# / .NET | Application development and business logic |
| CsvHelper | Parsing Google Forms CSV exports |
| Google Drive API | Retrieving uploaded photographs |
| Google OAuth 2.0 | Authenticating Google Drive access |
| HTML / CSS | Defining booklet layouts and formatting |
| PeachPDF | Rendering HTML into PDF documents |
| SkiaSharp | Measuring text for adaptive font sizing |

## ⚙️ How It Works

The application follows a multi-step document-generation workflow:

1. **Collect Submissions:** Participants complete a Google Form containing optional biographical information, personal stories, and photograph uploads.
2. **Export Data:** Google Forms responses are exported from Google Sheets into CSV format.
3. **Parse Submissions:** The application reads the CSV and converts each record into structured C# data models.
4. **Retrieve Photographs:** Submitted Google Drive photograph references are processed, with locally cached images reused when available.
5. **Generate Page Content:** The application selects and populates HTML templates based on the available submission data.
6. **Adjust Layouts:** CSS layouts and adaptive font-sizing logic accommodate different amounts of text and image dimensions.
7. **Generate PDF:** PeachPDF renders the resulting HTML into a multi-page PDF booklet.

### Document Layout

The booklet currently supports three primary types of histories:

**Family and Individual Histories**
- Family photographs and biographical information
- Parent biographies and children's names
- Wedding or alternative photographs
- Family stories and memories
- Temple photographs and personal significance

Family sections are designed to occupy either two or four pages, helping maintain consistent family boundaries for potential future book printing.

**Missionary Histories**
- Missionary information and photographs
- Mission locations and dates
- Mission experiences and personal stories
- Spiritual experiences and lessons learned

**Conversion Stories**
- Personal background and conversion experiences
- Photographs
- Stories about joining the Church

## 🖼️ PDF Output

The application generates PDF pages with a consistent visual design, including:

- Category-specific colors and headers
- Formatted biographies and story sections
- Automatically positioned photographs
- Adaptive text sizing
- Page numbering and identifying footers

Page content is generated conditionally so that optional information can be omitted without requiring participants to complete every field.

*Sample PDF screenshots can be added here to demonstrate the generated layouts.*

## 🚀 Getting Started

### Prerequisites

- .NET SDK compatible with the project
- Google Cloud project with the Google Drive API enabled
- Google OAuth 2.0 credentials
- CSV data exported from the expected Google Forms response structure

### Setup

1. Clone the repository:

   ```bash
   git clone https://github.com/LarrySon786/Wooster-Ward-History-Booklet.git
   ```

2. Navigate to the project directory.

3. Restore the project's dependencies:

   ```bash
   dotnet restore
   ```

4. Configure Google Drive API authentication.

   The application uses a local `credentials.json` file containing Google OAuth client credentials.

   **Important:** Credentials, authentication tokens, environment variables, and other sensitive files should not be committed to source control.

5. Prepare a CSV file matching the application's expected submission structure. See Models in project to find included data.

6. Configure the required local file paths and input data.

7. Run the application:

   ```bash
   dotnet run --project WoosterHistoryBooklet
   ```

   Depending on your environment, additional local configuration may be required.

## 🔒 Security and Privacy

This application is designed to process personal histories and photographs, so protecting submitted information is important.

- Google Drive authentication uses OAuth 2.0.
- Authentication credentials and tokens are stored outside source control.
- Downloaded photographs are cached locally.
- Personal submission data should not be committed to the public repository.
- Public examples should use fictional or appropriately authorized information.

The project is intended for a controlled, locally operated document-generation workflow rather than a publicly hosted submission platform.

## 🧩 Development Challenges

Several aspects of this project required particular attention.

### Dynamic PDF Layouts

One of the largest challenges was accommodating submissions of different lengths within fixed-size PDF pages.

Unlike a conventional webpage, a printed document has strict page boundaries. The application uses a combination of CSS Flexbox, conditional page generation, and text measurement to handle these constraints.

### Adaptive Font Sizing

Character counts alone are insufficient for determining whether text fits within a specific area, since different characters occupy different amounts of space.

To address this, the application uses text measurement to estimate the required space and adjust font sizes when necessary.

### Photograph Handling

Submitted photographs vary significantly in orientation, resolution, and aspect ratio.

The application uses image containers and CSS sizing rules to preserve photograph proportions while positioning them within the available page space.

### Optional Submission Data

Participants are not required to complete every field. This creates numerous possible combinations of available and missing content.

The application generates HTML sections conditionally and uses fallback layouts to maintain consistent booklet formatting.

## 🔮 Future Improvements

- Additional layout refinements for submissions with limited content
- Optional decorative photographs or quotations to balance whitespace
- More comprehensive error handling and validation
- Additional automated testing for unusual submission data
- Improved image fallback handling
- Support for preparing completed PDFs for physical book printing and binding

## 🎯 Project Purpose

This project combines my interest in software development with an opportunity to create something meaningful for my local community.

It has also provided practical experience working with external APIs, authentication, structured data, file management, dynamic HTML generation, CSS layouts, and PDF rendering.

The goal is to make it easier to preserve personal histories while reducing the time and effort required to assemble a consistently formatted booklet.

---

**Developed by Brandon Richards**

Software Development Student | C# | .NET | Backend Development
