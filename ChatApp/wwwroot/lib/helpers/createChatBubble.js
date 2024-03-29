﻿function createChatBubble(message, senderId, senderAvatar) {
    console.log("Message Sender ID:", message);
    console.log("Sender ID:", senderId);
    return (message.senderId == "SYSTEM")
        ?
        `
                <div class="d-flex flex-row justify-content-center" >
                    <div>
                        <p class="small p-2 ms-3 mb-1 rounded-3" style = "background-color: #a3ffb4;" >
                            ${message.content}
                        </p>
             
                    </div>
                </div>
        `
        : (message.senderId == senderId)
            ?
            `
                    <div class="d-flex flex-row justify-content-end" >
                        <div>
                            <p class="small p-2 me-3 mb-1 text-white rounded-3 bg-primary" >
                                ${message.content}
                            </p>
                            <p class="small me-3 mb-3 rounded-3 text-muted">${message.date}</p>
            `
            :
            `
                    <div class="d-flex flex-row justify-content-start" >
                    <img src="${senderAvatar}" alt="avatar"
                    class="rounded-circle d-flex align-self-center me-3 shadow-1-strong"
                            style="width: 60px; height: 60px; object-fit:cover
                        <div>
                            <p class="small p-2 ms-3 mb-1 rounded-3" style = "background-color: #f5f6f7;" >
                                ${message.content}
                            </p>
                            <p class="small ms-3 mb-3 rounded-3 text-muted float-end">${message.date}</p>
                        </div>
                    </div>
                    <div class="d-flex flex-row justify-content-start" >
                        <div style="width: 40px;"></div>
                        <p class="small ms-3 mb-3 rounded-3 text-muted">${message.date}</p>
                    </div>
            `;
}