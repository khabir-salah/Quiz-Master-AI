<h1>AI.Quiz</h1>

<p><strong>AI.Quiz</strong> is an innovative educational platform designed to automate the generation of assessment questions. With AI.Quiz, users can easily generate quiz questions based on text, document uploads, or pictures. Whether the user needs multiple-choice questions or true/false questions, AI.Quiz simplifies the process. Additionally, the platform allows users to share their assessments with others, offering a seamless way to generate and take assessments.</p>

<p>AI.Quiz also offers a <strong>subscription plan</strong> for accessing advanced features like document and picture-based question generation. The free <strong>Basic Plan</strong> allows users to generate only text-based questions with certain limitations, while the paid plans unlock more features.</p>

<hr>

<h2>Overview</h2>

<p>AI.Quiz provides a unique and interactive way to create quizzes from various sources. It is ideal for students, teachers, and anyone needing to generate questions quickly and efficiently. The platform supports sharing assessments, allowing users to challenge friends, classmates, or colleagues. The results of these assessments are available instantly after submission.</p>

<p>Whether it’s generating questions from a paragraph of text, a document, or even a picture, AI.Quiz has you covered. This flexibility makes it the perfect tool for educational purposes, quiz creation, or even exam preparation.</p>

<hr>

<h2>Features</h2>

<ul>
  <li><strong>AI-Powered Question Generation</strong>:
    <ul>
      <li>Generate quiz questions from <strong>text</strong>, <strong>documents</strong>, or <strong>pictures</strong>.</li>
      <li>Multiple-choice questions or true/false questions can be generated.</li>
    </ul>
  </li>
  <li><strong>Assessment Sharing</strong>:
    <ul>
      <li>Share your quiz with others and allow them to take your assessments.</li>
      <li>View results immediately after completion.</li>
    </ul>
  </li>
  <li><strong>Subscription Plans</strong>:
    <ul>
      <li><strong>Basic Plan (Free)</strong>: Generate up to 3 text-based questions per day.</li>
      <li><strong>Classic Plan</strong>: Access to generating <strong>10 text</strong> questions, <strong>5 document</strong> questions daily.</li>
      <li><strong>Standard Plan</strong>: Generate <strong>text, document</strong>, and <strong>picture-based</strong> questions.</li>
    </ul>
  </li>
  <li><strong>Subscription Payment Integration</strong>: Integrated with <strong>Paystack</strong> for seamless payment processing.</li>
  <li><strong>Authentication</strong>:
    <ul>
      <li>User registration and login.</li>
      <li>Role-based access control using <strong>Identity Framework</strong> and <strong>JWT</strong> for secure user management.</li>
    </ul>
  </li>
  <li><strong>Results and Feedback</strong>:
    <ul>
      <li>Users can view results instantly.</li>
      <li>Feedback is given after completing the assessments.</li>
    </ul>
  </li>
</ul>

<hr>

<h2>Technologies Used</h2>

<ul>
  <li><strong>Blazor WebAssembly</strong> for frontend development.</li>
  <li><strong>ASP.NET Core</strong> for backend services.</li>
  <li><strong>Identity Framework</strong> for user authentication and role management.</li>
  <li><strong>JWT</strong> (JSON Web Tokens) for secure authentication.</li>
  <li><strong>C#</strong> as the core programming language.</li>
  <li><strong>HTML/CSS</strong> for styling.</li>
  <li><strong>MySQL</strong> as the database.</li>
  <li><strong>CQRS</strong> and <strong>Onion Architecture</strong> to ensure scalability and maintainability.</li>
  <li><strong>Paystack</strong> for payment integration to handle subscriptions.</li>
</ul>

<hr>

<h2>Getting Started</h2>

<h3>Prerequisites</h3>

<p>To get started with the AI.Quiz application, you will need:</p>

<ul>
  <li><strong>.NET 7 SDK</strong> installed on your machine.</li>
  <li><strong>MySQL</strong> installed for database management.</li>
  <li><strong>Paystack</strong> account for payment integration.</li>
  <li><strong>Git</strong> installed to clone the repository.</li>
</ul>

<h3>Setup Instructions</h3>

<ol>
  <li><strong>Clone the Repository:</strong>
    <pre><code>git clone https://github.com/&lt;YourGitHubUsername&gt;/AI.Quiz.git
cd AI.Quiz
</code></pre>
  </li>
  <li><strong>Set up MySQL Database:</strong>
    <ul>
      <li>Create a MySQL database named <code>aiquizdb</code>.</li>
      <li>Update the <strong>connection string</strong> in the <code>appsettings.json</code> file:
        <pre><code>"ConnectionStrings": {
   "DefaultConnection": "Server=localhost;Database=aiquizdb;User Id=root;Password=yourpassword;"
}
</code></pre>
      </li>
    </ul>
  </li>
  <li><strong>Install the Required Packages:</strong>
    <pre><code>dotnet restore</code></pre>
  </li>
  <li><strong>Run Migrations:</strong>
    <pre><code>dotnet ef database update</code></pre>
  </li>
  <li><strong>Configure Paystack API:</strong>
    <ul>
      <li>Set up your <strong>Paystack public and secret keys</strong> in the <code>appsettings.json</code>:
        <pre><code>"Paystack": {
   "PublicKey": "your_public_key",
   "SecretKey": "your_secret_key"
}</code></pre>
      </li>
    </ul>
  </li>
  <li><strong>Run the Application:</strong>
    <pre><code>dotnet run</code></pre>
    <p>Navigate to <a href="https://localhost:5001">https://localhost:5001</a> in your browser.</p>
  </li>
</ol>

<hr>

<h2>Screenshots</h2>

<p><em>Here are some screenshots of the AI.Quiz application:</em></p>

<ul>
  <li><strong>Home Page:</strong>
    <p><em>(Screenshot of homepage showcasing the question generation options)</em></p>
  </li>
  <li><strong>Subscription Plans:</strong>
    <p><em>(Screenshot showing the subscription plans)</em></p>
  </li>
  <li><strong>Assessment Result:</strong>
    <p><em>(Screenshot displaying the user's assessment result after taking a quiz)</em></p>
  </li>
</ul>

<hr>

<h2>Contribution</h2>

<p>Feel free to fork the repository and make pull requests. Contributions are always welcome!</p>

<hr>

<h2>Contact</h2>

<p>For any inquiries or support, reach out at <strong>abdulkabirsalahudeen19@gmail.com</strong> or create an issue in the GitHub repository.</p>


