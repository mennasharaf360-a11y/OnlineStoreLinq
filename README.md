<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>HTML Form Example</title>
  <style>
    :root {
      --burgundy: #6d1a36;
      --burgundy-dark: #4a0f24;
      --beige: #f2e4cc;
      --beige-light: #fbf3e4;
      --beige-dark: #e3cfae;
      --text: #3b0d1e;
    }

    * { box-sizing: border-box; }

    body {
      margin: 0;
      padding: 40px 16px;
      font-family: "Segoe UI", Tahoma, Arial, sans-serif;
      background: var(--beige);
      color: var(--text);
      font-weight: 600;
      line-height: 1.6;
    }

    .container {
      max-width: 720px;
      margin: 0 auto;
    }

    h1 {
      margin: 0 0 8px;
      color: var(--burgundy);
      font-size: 2.4rem;
      font-weight: 800;
    }

    p.intro {
      margin: 0 0 26px;
      font-weight: 700;
    }

    del { color: var(--burgundy); text-decoration-thickness: 2px; }
    u { text-decoration-color: var(--burgundy); text-decoration-thickness: 2px; text-underline-offset: 3px; }
    mark { background: var(--burgundy); color: var(--beige-light); padding: 1px 6px; border-radius: 4px; }

    fieldset {
      background: var(--beige-light);
      border: 3px solid var(--burgundy);
      border-radius: 14px;
      padding: 18px 22px 22px;
      margin: 0 0 22px;
      box-shadow: 6px 6px 0 var(--burgundy);
    }

    legend {
      padding: 5px 16px;
      background: var(--burgundy);
      color: var(--beige-light);
      border: 3px solid var(--burgundy-dark);
      border-radius: 20px;
      font-weight: 800;
      font-size: 0.95rem;
    }

    label {
      display: inline-block;
      min-width: 90px;
      font-weight: 700;
      color: var(--burgundy-dark);
    }

    fieldset label[for="newsletter1"],
    fieldset label[for="newsletter2"],
    fieldset label[for="language"],
    fieldset label[for="rating"] {
      min-width: 0;
      margin-inline-end: 6px;
    }

    input[type="text"],
    input[type="email"],
    input[type="password"],
    input[type="tel"],
    textarea,
    select {
      padding: 9px 12px;
      border: 2.5px solid var(--burgundy);
      border-radius: 8px;
      font-size: 0.95rem;
      font-family: inherit;
      font-weight: 600;
      color: var(--text);
      background: #fffaf0;
      transition: box-shadow 0.2s, background 0.2s;
    }

    input[type="text"],
    input[type="email"],
    input[type="password"],
    input[type="tel"] {
      width: 260px;
      max-width: 100%;
    }

    textarea {
      width: 100%;
      margin-top: 6px;
      resize: vertical;
    }

    input:focus,
    textarea:focus,
    select:focus {
      outline: none;
      background: #fff;
      box-shadow: 0 0 0 4px rgba(109, 26, 54, 0.3);
    }

    input[type="checkbox"],
    input[type="range"] {
      accent-color: var(--burgundy);
    }

    input[type="checkbox"] {
      width: 20px;
      height: 20px;
      vertical-align: middle;
    }

    input[type="range"] {
      vertical-align: middle;
      width: 220px;
    }

    .buttons { margin-top: 8px; }

    input[type="submit"],
    input[type="reset"] {
      padding: 10px 30px;
      border: 3px solid var(--burgundy);
      border-radius: 8px;
      font-size: 1rem;
      font-weight: 800;
      cursor: pointer;
      box-shadow: 4px 4px 0 var(--burgundy-dark);
      transition: background 0.2s, transform 0.1s, box-shadow 0.1s;
    }

    input[type="submit"] {
      background: var(--burgundy);
      color: var(--beige-light);
    }
    input[type="submit"]:hover { background: var(--burgundy-dark); }

    input[type="reset"] {
      background: var(--beige-light);
      color: var(--burgundy);
      margin-inline-start: 10px;
    }
    input[type="reset"]:hover { background: var(--beige-dark); }

    input[type="submit"]:active,
    input[type="reset"]:active {
      transform: translate(3px, 3px);
      box-shadow: 1px 1px 0 var(--burgundy-dark);
    }

    /* دعم الاتجاه من اليمين لليسار */
    [dir="rtl"] fieldset { box-shadow: -6px 6px 0 var(--burgundy); }
    [dir="rtl"] input[type="submit"],
    [dir="rtl"] input[type="reset"] { box-shadow: -4px 4px 0 var(--burgundy-dark); }
    [dir="rtl"] input[type="submit"]:active,
    [dir="rtl"] input[type="reset"]:active {
      transform: translate(-3px, 3px);
      box-shadow: -1px 1px 0 var(--burgundy-dark);
    }

    @media (max-width: 520px) {
      label { display: block; margin-bottom: 4px; }
    }
  </style>
</head>
<body>
  <div class="container">
    <h1 data-i18n="title">HTML Form Example</h1>

    <p class="intro" data-i18n-html="intro">
      This is an example of an <del>HTML4</del> <u>HTML5</u> form containing يحتوي various
      <mark>input</mark> controls:
    </p>

    <form action="#" method="post">

      <fieldset>
        <legend data-i18n="legendUser">User Information</legend>
        <label for="username" data-i18n="username">Username:</label>
        <input type="text" id="username" name="username">
        <br><br>
        <label for="email" data-i18n="email">Email:</label>
        <input type="email" id="email" name="email">
        <br><br>
        <label for="password" data-i18n="password">Password:</label>
        <input type="password" id="password" name="password">
      </fieldset>

      <fieldset>
        <legend data-i18n="legendContact">Contact Information</legend>
        <label for="phone" data-i18n="phone">Phone:</label>
        <input type="tel" id="phone" name="phone">
        <br><br>
        <label for="message" data-i18n="message">Message:</label>
        <br>
        <textarea id="message" name="message" rows="4"></textarea>
      </fieldset>

      <fieldset>
        <legend data-i18n="legendSub">Subscription</legend>
        <label for="newsletter1" data-i18n="newsletter">Subscribe to newsletter:</label>
        <input type="checkbox" id="newsletter1" name="newsletter1">
        <br>
        <label for="newsletter2" data-i18n="newsletter">Subscribe to newsletter:</label>
        <input type="checkbox" id="newsletter2" name="newsletter2">
      </fieldset>

      <fieldset>
        <legend data-i18n="legendLang">Preferred Language</legend>
        <label for="language" data-i18n="selectLang">Select your preferred language:</label>
        <select id="language" name="language">
          <option value="en">English</option>
          <option value="es">Spanish</option>
          <option value="fr">French</option>
          <option value="de">German</option>
          <option value="ar">Arabic</option>
        </select>
      </fieldset>

      <fieldset>
        <legend data-i18n="legendFeedback">Feedback</legend>
        <label for="rating" data-i18n="rate">Rate our service:</label>
        <input type="range" id="rating" name="rating" min="1" max="10" value="5">
      </fieldset>

      <div class="buttons">
        <input type="submit" id="submitBtn" value="Submit">
        <input type="reset" id="resetBtn" value="Reset">
      </div>

    </form>
  </div>

  <script>
    const translations = {
      en: {
        title: "HTML Form Example",
        intro: 'This is an example of an <del>HTML4</del> <u>HTML5</u> form containing يحتوي various <mark>input</mark> controls:',
        legendUser: "User Information", username: "Username:", email: "Email:", password: "Password:",
        legendContact: "Contact Information", phone: "Phone:", message: "Message:",
        legendSub: "Subscription", newsletter: "Subscribe to newsletter:",
        legendLang: "Preferred Language", selectLang: "Select your preferred language:",
        legendFeedback: "Feedback", rate: "Rate our service:",
        submit: "Submit", reset: "Reset"
      },
      es: {
        title: "Ejemplo de formulario HTML",
        intro: 'Este es un ejemplo de un formulario <del>HTML4</del> <u>HTML5</u> que contiene varios controles de <mark>entrada</mark>:',
        legendUser: "Información del usuario", username: "Nombre de usuario:", email: "Correo electrónico:", password: "Contraseña:",
        legendContact: "Información de contacto", phone: "Teléfono:", message: "Mensaje:",
        legendSub: "Suscripción", newsletter: "Suscribirse al boletín:",
        legendLang: "Idioma preferido", selectLang: "Seleccione su idioma preferido:",
        legendFeedback: "Comentarios", rate: "Califique nuestro servicio:",
        submit: "Enviar", reset: "Restablecer"
      },
      fr: {
        title: "Exemple de formulaire HTML",
        intro: 'Ceci est un exemple de formulaire <del>HTML4</del> <u>HTML5</u> contenant divers contrôles de <mark>saisie</mark> :',
        legendUser: "Informations utilisateur", username: "Nom d'utilisateur :", email: "E-mail :", password: "Mot de passe :",
        legendContact: "Coordonnées", phone: "Téléphone :", message: "Message :",
        legendSub: "Abonnement", newsletter: "S'abonner à la newsletter :",
        legendLang: "Langue préférée", selectLang: "Sélectionnez votre langue préférée :",
        legendFeedback: "Avis", rate: "Évaluez notre service :",
        submit: "Envoyer", reset: "Réinitialiser"
      },
      de: {
        title: "HTML-Formularbeispiel",
        intro: 'Dies ist ein Beispiel für ein <del>HTML4</del>- <u>HTML5</u>-Formular mit verschiedenen <mark>Eingabe</mark>-Steuerelementen:',
        legendUser: "Benutzerinformationen", username: "Benutzername:", email: "E-Mail:", password: "Passwort:",
        legendContact: "Kontaktinformationen", phone: "Telefon:", message: "Nachricht:",
        legendSub: "Abonnement", newsletter: "Newsletter abonnieren:",
        legendLang: "Bevorzugte Sprache", selectLang: "Wählen Sie Ihre bevorzugte Sprache:",
        legendFeedback: "Feedback", rate: "Bewerten Sie unseren Service:",
        submit: "Absenden", reset: "Zurücksetzen"
      },
      ar: {
        title: "مثال على نموذج HTML",
        intro: 'هذا مثال على نموذج <del>HTML4</del> <u>HTML5</u> يحتوي على عناصر <mark>إدخال</mark> متنوعة:',
        legendUser: "معلومات المستخدم", username: "اسم المستخدم:", email: "البريد الإلكتروني:", password: "كلمة المرور:",
        legendContact: "معلومات التواصل", phone: "رقم الهاتف:", message: "الرسالة:",
        legendSub: "الاشتراك", newsletter: "الاشتراك في النشرة البريدية:",
        legendLang: "اللغة المفضلة", selectLang: "اختر لغتك المفضلة:",
        legendFeedback: "التقييم", rate: "قيّم خدمتنا:",
        submit: "إرسال", reset: "إعادة تعيين"
      }
    };

    const langSelect = document.getElementById("language");

    function setLanguage(lang) {
      const t = translations[lang];
      document.documentElement.lang = lang;
      document.documentElement.dir = (lang === "ar") ? "rtl" : "ltr";

      document.querySelectorAll("[data-i18n]").forEach(el => {
        el.textContent = t[el.dataset.i18n];
      });
      document.querySelectorAll("[data-i18n-html]").forEach(el => {
        el.innerHTML = t[el.dataset.i18nHtml];
      });

      document.getElementById("submitBtn").value = t.submit;
      document.getElementById("resetBtn").value = t.reset;
    }

    langSelect.addEventListener("change", () => setLanguage(langSelect.value));

    // بعد Reset: القايمة بترجع للقيمة الأصلية، فنرجّع لغة الصفحة تتطابق معها
    document.querySelector("form").addEventListener("reset", () => {
      setTimeout(() => setLanguage(langSelect.value), 0);
    });
  </script>
</body>
</html>
