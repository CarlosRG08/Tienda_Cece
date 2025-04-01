(function ($) {
    "use strict";

    // Spinner
    var spinner = function () {
        setTimeout(function () {
            if ($('#spinner').length > 0) {
                $('#spinner').removeClass('show');
            }
        }, 1);
    };
    spinner(0);

    //function heart
    $(document).ready(function () {
        $('.heart-icon').click(function () {
            if ($(this).hasClass('fa-regular')) {
                $(this).removeClass('fa-regular').addClass('fa-solid');
                $(this).css('color', '#b42727');
            } else {
                $(this).removeClass('fa-solid').addClass('fa-regular');
                $(this).css('color', '#000');
            }
        });
    });

    // Fixed Navbar
    $(window).scroll(function () {
        if ($(window).width() < 992) {
            if ($(this).scrollTop() > 55) {
                $('.fixed-top').addClass('shadow');
            } else {
                $('.fixed-top').removeClass('shadow');
            }
        } else {
            if ($(this).scrollTop() > 55) {
                $('.fixed-top').addClass('shadow').css('top', -25);
            } else {
                $('.fixed-top').removeClass('shadow').css('top', 0);
            }
        }
    });

    // Back to top button
    $(window).scroll(function () {
        if ($(this).scrollTop() > 300) {
            $('.back-to-top').fadeIn('slow');
        } else {
            $('.back-to-top').fadeOut('slow');
        }
    });
    $('.back-to-top').click(function () {
        $('html, body').animate({ scrollTop: 0 }, 1500, 'easeInOutExpo');
        return false;
    });


    // Testimonial carousel
    $(".testimonial-carousel").owlCarousel({
        autoplay: true,
        smartSpeed: 2000,
        center: false,
        dots: true,
        loop: true,
        margin: 25,
        nav: true,
        navText: [
            '<i class="bi bi-arrow-left"></i>',
            '<i class="bi bi-arrow-right"></i>'
        ],
        responsiveClass: true,
        responsive: {
            0: {
                items: 1
            },
            576: {
                items: 1
            },
            768: {
                items: 1
            },
            992: {
                items: 2
            },
            1200: {
                items: 2
            }
        }
    });


    // vegetable carousel
    $(".vegetable-carousel").owlCarousel({
        autoplay: true,
        smartSpeed: 1500,
        center: false,
        dots: true,
        loop: true,
        margin: 25,
        nav: true,
        navText: [
            '<i class="bi bi-arrow-left"></i>',
            '<i class="bi bi-arrow-right"></i>'
        ],
        responsiveClass: true,
        responsive: {
            0: {
                items: 1
            },
            576: {
                items: 1
            },
            768: {
                items: 2
            },
            992: {
                items: 3
            },
            1200: {
                items: 4
            }
        }
    });


    // Modal Video
    $(document).ready(function () {
        var $videoSrc;
        $('.btn-play').click(function () {
            $videoSrc = $(this).data("src");
        });
        console.log($videoSrc);

        $('#videoModal').on('shown.bs.modal', function (e) {
            $("#video").attr('src', $videoSrc + "?autoplay=1&amp;modestbranding=1&amp;showinfo=0");
        })

        $('#videoModal').on('hide.bs.modal', function (e) {
            $("#video").attr('src', $videoSrc);
        })
    });



    // Product Quantity
    $('.quantity button').on('click', function () {
        var button = $(this);
        var oldValue = button.parent().parent().find('input').val();
        if (button.hasClass('btn-plus')) {
            var newVal = parseFloat(oldValue) + 1;
        } else {
            if (oldValue > 0) {
                var newVal = parseFloat(oldValue) - 1;
            } else {
                newVal = 0;
            }
        }
        button.parent().parent().find('input').val(newVal);
    });

})(jQuery);


//CRUDS
$(document).ready(function () {
    // Activate tooltip
    $('[data-toggle="tooltip"]').tooltip();

    // Select/Deselect checkboxes
    var checkbox = $('table tbody input[type="checkbox"]');
    $("#selectAll").click(function () {
        if (this.checked) {
            checkbox.each(function () {
                this.checked = true;
            });
        } else {
            checkbox.each(function () {
                this.checked = false;
            });
        }
    });
    checkbox.click(function () {
        if (!this.checked) {
            $("#selectAll").prop("checked", false);
        }
    });
});


// main.js
// chat bot

function toggleChat() {
    var chatContainer = document.getElementById('chatbotContainer');
    chatContainer.style.display = chatContainer.style.display === 'block' ? 'none' : 'block';
    if (chatContainer.style.display === 'block' && !chatContainer.classList.contains('initialized')) {
        initializeChat();
        chatContainer.classList.add('initialized');
    }
}

function initializeChat() {
    clearMessages();
    addMessage("¡Hola! ¿Cómo puedo ayudarte hoy?\nPor favor, elige una opción escribiendo el número correspondiente:", "bot");
    addMessage("1. Información sobre los métodos de pago", "bot");
    addMessage("2. Información sobre mis pedidos realizados", "bot");
    addMessage("3. Cómo puedo contactarme con la tienda? ", "bot");
}

function sendMessage() {
    var input = document.getElementById('userInput');
    var message = input.value.trim();
    if (message !== "") {
        addMessage(message, "user");
        input.value = "";
        handleUserMessage(message);
    }
}

function handleUserMessage(message) {
    var response;
    switch (message) {
        case "1":
            response = "Tenemos dos formas de pago:\n1) Pago por SINPE en la pagina Web: Ingrese los productos deseados al carrito, luego seleccione la opción de continuar al pago, realiza la transferencia al número que te indicamos y adjuntas una captura del comprobante.\n2)Pago por medio de WhatsApp: Contacte la tienda por medio de WhatsApp y ahí le brindaremos la información para que realice el pago.";
            break;
        case "2":
            response = "Para observar tus pedidos te puedes ir a tu perfil y en tu perfil hay una opción que dice mis pedidos, al ingresar veras una lista de todos tus pedidos realizados.";
            break;
        case "3":
            response = "Tienes muchas formas de contactarte con nosotros te puedes dirigir a la pestaña de contacto y te apareceran todos los medios por el cual puedes comunicarte con nosotros o tambien en la parte inferior de nuestra pagina tambien encontraras todas nuestras redes sociales y correo electronico, incluso arriba de mi boton de Chat Bot encontraras uno para WhatsApp que te enviara directo a nuestro Chat.Espero haberte ayudado.";
            break;
        default:
            response = "No entendí tu solicitud. Por favor, elige una opción válida del menú que te ofreci antriormente.";
            setTimeout(initializeChat, 1500);
            return;
    }
    setTimeout(() => {
        addMessage(response, "bot");
    }, 1000);
}

function addMessage(message, sender) {
    var messages = document.getElementById('messages');
    var messageElement = document.createElement('div');
    messageElement.classList.add('message');
    messageElement.classList.add(sender === 'bot' ? 'bot' : 'user');
    messageElement.textContent = message;
    messages.appendChild(messageElement);
    messages.scrollTop = messages.scrollHeight;
}

function clearMessages() {
    var messages = document.getElementById('messages');
    messages.innerHTML = "";
}

function handleKeyPress(event) {
    if (event.key === 'Enter') {
        sendMessage();
    }
}


$(document).ready(function () {
    function procesarPago() {
        $('#paymentProcessedModal').modal('show');
    }

    $('.btn.bg-secondary.px-4.py-2.mb-4.text-light').click(function () {
        procesarPago();
    });
});
