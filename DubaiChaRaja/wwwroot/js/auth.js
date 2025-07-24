console.log("no auth.js loaded");


$(document).ready(function () {

    $(document).on("submit", "#loginForm", function (e) {
        e.preventDefault();

        const data = {
            username: this.username.value,
            password: this.password.value
        };

        $.ajax({
            url: "/Auth/Login",
            type: "POST",
            data: data,
            success: function (res) {
                console.log(" Logged in!");

                window.location.href = res.redirect;
            },
            error: function () {
                $("#loginError").text("Invalid credentials.");
            }
        });
    });

    let isEmailVerified = false;




    $(document).on("change", "#codeBox", function () {
        const code = $(this).val();
        const email = $("#registerEmail").val();

        $.ajax({
            url: "/Auth/VerifyCode",
            type: "POST",
            data: { email: email, code: code },
            success: function () {
                isEmailVerified = true;
                $("#codeMag").text("Email Verified!");
            },
            error: function () {
                isEmailVerified = false;
                $("#codeMag").text(" Invalid or expired code.");
            }
        });
    });

    $(document).on("click", "#sendCodeBtn", function (e) {
        e.preventDefault();
        e.stopPropagation();


        const email = $("#registerEmail").val();


        if (!email) {
            alert("Please enter your email.");
            return;
        }

        $.post("/Auth/SendCode", { email })
            .done(() => {
                alert("Verification code sent!");
                $("#codeInputGroup").show();
            })
            .fail(() => {
                alert(" Failed to send verification code.");
            });
    });

    $(document).on("click", "#forgotSendBtn", function () {
        const email = $("#forgotEmail").val();
        console.log("checked email ");
        if (!email) return alert("Please enter email");


        $.post("/Auth/SendCode", { email })
            .done(() => {
                $("#forgotMsg").text("verification code sent!");
                console.log
            })
            .fail(() => {
                $("#forgotMsg").text("Something went Wrong");
            });
    });


    $(document).on("submit", "#forgotForm", function (e) {
        e.preventDefault();



        const email = $("#forgotEmail").val();
        const code = $("#forgotCode").val();
        const newPassword = this.newPassword.value;

        $.post("/Auth/ForgotPassword", { email, newPassword })
            .done(() => {
                alert(" Password updated!.");
                window.location.href = "/Home/Welcome";
            })
            .fail(() => {
                alert("Invalid code or error updating password.");
            });

    });



    $(document).on("submit", "#registerForm", function (e) {
        e.preventDefault();

        if (!isEmailVerified) {
            $("#registerError").text("Please verify your email before registering.");
            return;
        }

        const data = {
            username: this.username.value,
            email: this.email.value,
            password: this.password.value,
            code: $("#codeBox").val()
        };

        $.ajax({
            url: "/Auth/Register",
            type: "POST",
            data: data,
            success: function () {
                alert("Registered successfully!");
                $("#registerModal").modal("hide");
                isEmailVerified = false;
                $("#registerForm")[0].reset();
                $("#codeInputGroup").hide();
                $("#codeMag").text("");
                $("#registerError").text("");
            },
            error: function () {
                $("#registerError").text("User already exists/ Email already exists  or verification failed.");
            }
        });
    });
});

