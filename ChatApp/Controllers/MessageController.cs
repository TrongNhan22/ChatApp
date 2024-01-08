using ChatApp.Models;
using Microsoft.AspNetCore.Mvc;
using ChatApp.ViewModels;
using ChatApp.Interface;
using Microsoft.Extensions.Primitives;

namespace ChatApp.Controllers
{
    public class MessageController : Controller
    {
        private readonly IMessageRepository _messageRepository;

        public MessageController(IMessageRepository messageService)
        {
            _messageRepository = messageService;
        }
        [HttpGet]
        public async Task<IActionResult>Index()
        {
            User? currentUser = Globals.user_login;
            if (Globals.user_login == null)
            {
                return Redirect("~/Login");
            }
            //For testing
            //string senderId = "65340b20b32df212d36f15ad";
            string senderId = currentUser.id!;
            var chatPartnerList = await _messageRepository.GetChatPartner();
            var chatPartner = chatPartnerList.FirstOrDefault()?.id;
            IEnumerable<Message>? messages = chatPartner == null 
                ? null 
                : await _messageRepository.GetListByChatParticipantId(senderId, chatPartner);
            var chatViewModel = new ChatViewModel
            {
                SenderId = senderId,
                ReceiverId = chatPartner,
                Messages = messages,
                ChatPartners = chatPartnerList
            };
            return View(chatViewModel);

        }
        //public async Task<IActionResult> Post(MessageViewModel messageViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var message = new Message
        //        { 
        //            SenderId = messageViewModel.SenderId,
        //            ReceiverId = messageViewModel.ReceiverId,
        //            Content = messageViewModel.Content,
        //            Media=messageViewModel.Media,
        //            Date = DateTime.Now.ToString(),
        //        };
        //        await _messageRepository.CreateAsync(message);
        //    }
        //    //if (!ModelState.IsValid)
        //    //{
        //    //    return View();
        //    //}
        //    //return RedirectToAction("Index");
        //    //_messageService.CreateAsync(newMessage);
        //    //await _messageService.CreateAsync(newMessage);
        //    //return CreatedAtAction(nameof(Get), new { id = newMessage.Id }, newMessage);

        //}
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Message updatedMessage)
        {
            var message = await _messageRepository.GetMessageByIdAsync(id);
            if (message is null) return NotFound();
            updatedMessage.Id = message.Id;
            await _messageRepository.UpdateAsync(id, updatedMessage);
            return NoContent();
        }
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var message = await _messageRepository.GetMessageByIdAsync(id);
            if (message is null) return NotFound();
            await _messageRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}