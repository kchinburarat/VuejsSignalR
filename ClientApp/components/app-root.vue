<template>
  <div class="form-group">
    <form>
      <div>
        <label for="userName">Your Name</label>
        <input type="text" name="Name" id="userName" v-model="userName">
      </div>
      <div>
        <label for="userMessage">Message</label>
        <input type="text" name="Message" id="userMessage" v-model="userMessage">
      </div>
      <button v-on:click="submitCard" type="button">Submit</button>
    </form>
    <ul v-for="(item, index) in messages" v-bind:key="index + 'itemMessage'">
      <li><b>Name: </b>{{item.user}}</li>
      <li><b>Message: </b>{{item.message}}</li>
    </ul>
  </div>
</template>
<script>

  import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'

  export default {
    data: {
      userName: "",
      userMessage: "",
      connection: "",
      messages: [],
      startedPromise: null
    },
    methods: {
      submitCard: function () {
        if (this.userName && this.userMessage) {
          this.connection.invoke('SendMessage', this.userName, this.userMessage).catch(function (err) {
            return console.error(err.toSting());
          });

          this.userName = '';
          this.userMessage = '';
        }
      },
      start () {
        this.startedPromise = this.connection.start().catch(err => {
        console.error('Failed to connect with hub', err)
        return new Promise((resolve, reject) => 
          setTimeout(() => start().then(resolve).catch(reject), 5000))
        })
        return this.startedPromise 
      }
    }, 
    created: function () {
      this.connection = new HubConnectionBuilder()
        .withUrl('/chatHub')
        .configureLogging(LogLevel.Information)
        .build();
       this.connection.onclose(() => this.start())

    
    },
    mounted: function () {   
      var thisVue = this;
      thisVue.connection.start();
      thisVue.connection.on('ReceiveMessage', function (user, message) {
        thisVue.messages.push({ user, message });
      });
    }
  }
</script>
<style>

</style>
