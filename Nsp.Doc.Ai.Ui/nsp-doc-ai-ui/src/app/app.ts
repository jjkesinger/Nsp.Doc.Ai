import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule, HttpClientModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('Docu-Grok');
  protected readonly chatHistory: ChatHistory[] = [];

  constructor(private http: HttpClient) { }

  public addChat(inputElement: HTMLInputElement): void {
    const message = inputElement.value.trim();
    if (message) {
      this.chatHistory.push(new ChatHistory(message, false));
      this.askAssistant(message);
    }
    inputElement.value = '';
  }

  public onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      const formData = new FormData();
      formData.append('file', file);

      this.http.post<{ response: any }>('https://super-duper-telegram-pg4jgr646xv2rxw-5000.app.github.dev/upload', formData)
        .subscribe({
          next: (res: any) => {
            this.chatHistory.push(new ChatHistory(res.message, true));
            input.value = '';
          },
          error: (err) => {
            console.error('File upload error:', err);
            this.chatHistory.push(new ChatHistory('File upload failed.', true));
            input.value = '';
          }
        });
    }
  }

  private askAssistant(message: string): void {
    this.http.get<{ response: any }>('https://super-duper-telegram-pg4jgr646xv2rxw-5000.app.github.dev/ask?query=' + encodeURIComponent(message))
      .subscribe({
        next: (res: any) => {
          this.chatHistory.push(new ChatHistory(res.message, true));
        },
        error: (err) => {
          console.error('API error:', err);
          this.chatHistory.push(new ChatHistory('Sorry, something went wrong.', true));
        }
      });
  }
}

class ChatHistory {
  constructor(message: string, isAssistant: boolean = false) {
    this.message = message;
    this.isAssistant = isAssistant;
  }

  public message: string;
  public isAssistant: boolean;
}
