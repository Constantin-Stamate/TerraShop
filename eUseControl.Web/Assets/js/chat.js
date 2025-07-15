const messageForm = document.querySelector(".prompt__form");
const chatHistoryContainer = document.querySelector(".chats");
const suggestionItems = document.querySelectorAll(".suggests__item");

const themeToggleButton = document.getElementById("themeToggler");
const clearChatButton = document.getElementById("deleteButton");

let currentUserMessage = null;
let isGeneratingResponse = false;

const loadSavedChatHistory = () => {
    const savedConversations = JSON.parse(localStorage.getItem("saved-api-chats")) || [];
    const isLightTheme = localStorage.getItem("themeColor") === "light_mode";

    document.body.classList.toggle("light_mode", isLightTheme);
    themeToggleButton.innerHTML = isLightTheme ? '<i class="bx bx-moon"></i>' : '<i class="bx bx-sun"></i>';

    chatHistoryContainer.innerHTML = '';

    savedConversations.forEach(conversation => {
        const userMessageHtml = `
            <div class="message__content">
                <img class="message__avatar" src="/Assets/img/chat-icons/user.png" alt="User avatar">
                <p class="message__text">${conversation.userMessage}</p>
            </div>
        `;

        chatHistoryContainer.appendChild(createChatMessageElement(userMessageHtml, "message--outgoing"));

        const responseText = conversation.apiResponse?.responseText || '';
        const parsedApiResponse = marked.parse(responseText);

        const responseHtml = `
            <div class="message__content">
                <img class="message__avatar" src="/Assets/img/chat-icons/terra.svg" alt="Terra avatar">
                <p class="message__text">${parsedApiResponse}</p>
                <div class="message__loading-indicator hide">
                    <div class="message__loading-bar"></div>
                    <div class="message__loading-bar"></div>
                    <div class="message__loading-bar"></div>
                </div>
            </div>
            <span onClick="copyMessageToClipboard(this)" class="message__icon"><i class='bx bx-copy-alt'></i></span>
        `;

        const incomingMessageElement = createChatMessageElement(responseHtml, "message--incoming");
        chatHistoryContainer.appendChild(incomingMessageElement);
        chatHistoryContainer.scrollTop = chatHistoryContainer.scrollHeight;
    });

    hljs.highlightAll();
    addCopyButtonToCodeBlocks();

    document.body.classList.toggle("hide-header", savedConversations.length > 0);
};

const createChatMessageElement = (htmlContent, ...cssClasses) => {
    const messageElement = document.createElement("div");
    messageElement.classList.add("message", ...cssClasses);
    messageElement.innerHTML = htmlContent;
    return messageElement;
};

const showTypingEffect = (rawText, htmlText, messageElement, incomingMessageElement, skipEffect = false) => {
    const copyIconElement = incomingMessageElement.querySelector(".message__icon");
    copyIconElement.classList.add("hide");

    if (skipEffect) {
        messageElement.innerHTML = htmlText;
        hljs.highlightAll();
        addCopyButtonToCodeBlocks();
        copyIconElement.classList.remove("hide");
        isGeneratingResponse = false;
        return;
    }

    const wordsArray = rawText.split(' ');
    let wordIndex = 0;

    const typingInterval = setInterval(() => {
        messageElement.innerText += (wordIndex === 0 ? '' : ' ') + wordsArray[wordIndex++];
        if (wordIndex === wordsArray.length) {
            clearInterval(typingInterval);
            isGeneratingResponse = false;
            messageElement.innerHTML = htmlText;
            hljs.highlightAll();
            addCopyButtonToCodeBlocks();
            copyIconElement.classList.remove("hide");
        }
    }, 75);
};

const handleOutgoingMessage = async () => {
    currentUserMessage = messageForm.querySelector(".prompt__form-input").value.trim() || currentUserMessage;
    if (!currentUserMessage || isGeneratingResponse) return;

    isGeneratingResponse = true;

    const outgoingMessageHtml = `
        <div class="message__content">
            <img class="message__avatar" src="/Assets/img/chat-icons/user.png" alt="User avatar">
            <p class="message__text"></p>
        </div>
    `;

    const outgoingMessageElement = createChatMessageElement(outgoingMessageHtml, "message--outgoing");
    outgoingMessageElement.querySelector(".message__text").innerText = currentUserMessage;
    chatHistoryContainer.appendChild(outgoingMessageElement);

    messageForm.reset();
    document.body.classList.add("hide-header");

    const loadingHtml = `
        <div class="message__content">
            <img class="message__avatar" src="/Assets/img/chat-icons/terra.svg" alt="Terra avatar">
            <p class="message__text"></p>
            <div class="message__loading-indicator">
                <div class="message__loading-bar"></div>
                <div class="message__loading-bar"></div>
                <div class="message__loading-bar"></div>
            </div>
        </div>
        <span onClick="copyMessageToClipboard(this)" class="message__icon hide"><i class='bx bx-copy-alt'></i></span>
    `;

    const incomingMessageElement = createChatMessageElement(loadingHtml, "message--incoming", "message--loading");
    chatHistoryContainer.appendChild(incomingMessageElement);
    const messageTextElement = incomingMessageElement.querySelector(".message__text");

    try {
        const response = await fetch("/Chat/Respond", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ message: currentUserMessage })
        });

        if (!response.ok) throw new Error(`Server error: ${response.statusText}`);

        const data = await response.json();
        const responseText = data.responseText || "No response";

        const parsedApiResponse = marked.parse(responseText);
        showTypingEffect(responseText, parsedApiResponse, messageTextElement, incomingMessageElement);

        let savedConversations = JSON.parse(localStorage.getItem("saved-api-chats")) || [];
        savedConversations.push({
            userMessage: currentUserMessage,
            apiResponse: {
                responseText: responseText
            }
        });

        localStorage.setItem("saved-api-chats", JSON.stringify(savedConversations));
    } catch (error) {
        isGeneratingResponse = false;
        messageTextElement.innerText = error.message;
        messageTextElement.closest(".message").classList.add("message--error");
    } finally {
        incomingMessageElement.classList.remove("message--loading");
    }
};

const addCopyButtonToCodeBlocks = () => {
    const codeBlocks = document.querySelectorAll('pre');

    codeBlocks.forEach(block => {
        const codeElement = block.querySelector('code');
        let language = [...codeElement.classList].find(cls => cls.startsWith('language-'))?.replace('language-', '') || 'Text';

        const languageLabel = document.createElement('div');
        languageLabel.innerText = language.charAt(0).toUpperCase() + language.slice(1);
        languageLabel.classList.add('code__language-label');
        block.appendChild(languageLabel);

        const copyButton = document.createElement('button');
        copyButton.innerHTML = `<i class='bx bx-copy'></i>`;
        copyButton.classList.add('code__copy-btn');
        block.appendChild(copyButton);

        copyButton.addEventListener('click', () => {
            navigator.clipboard.writeText(codeElement.innerText).then(() => {
                copyButton.innerHTML = `<i class='bx bx-check'></i>`;
                setTimeout(() => copyButton.innerHTML = `<i class='bx bx-copy'></i>`, 2000);
            }).catch(err => {
                console.error("Copy failed:", err);
                alert("Unable to copy text!");
            });
        });
    });
};

const copyMessageToClipboard = (copyButton) => {
    const messageContent = copyButton.parentElement.querySelector(".message__text").innerText;
    navigator.clipboard.writeText(messageContent);
    copyButton.innerHTML = `<i class='bx bx-check'></i>`;
    setTimeout(() => copyButton.innerHTML = `<i class='bx bx-copy-alt'></i>`, 1000);
};

themeToggleButton.addEventListener('click', () => {
    const isLightTheme = document.body.classList.toggle("light_mode");
    localStorage.setItem("themeColor", isLightTheme ? "light_mode" : "dark_mode");

    const newIconClass = isLightTheme ? "bx bx-moon" : "bx bx-sun";
    themeToggleButton.querySelector("i").className = newIconClass;
});

clearChatButton.addEventListener('click', () => {
    if (confirm("Are you sure you want to delete all chat history?")) {
        localStorage.removeItem("saved-api-chats");
        loadSavedChatHistory();
        currentUserMessage = null;
        isGeneratingResponse = false;
    }
});

suggestionItems.forEach(suggestion => {
    suggestion.addEventListener('click', () => {
        currentUserMessage = suggestion.querySelector(".suggests__item-text").innerText;
        handleOutgoingMessage();
    });
});

messageForm.addEventListener('submit', e => {
    e.preventDefault();
    handleOutgoingMessage();
});

loadSavedChatHistory();