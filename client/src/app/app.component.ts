import { HttpClient } from '@angular/common/http';
import { Component, OnInit, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {
  title = 'client';
  http = inject(HttpClient);
  ngOnInit(): void {
    this.http.get('http://127.0.0.1:5000/api/Users').subscribe({
      next: (response) => {
        console.log('response', response);
      },
    });
  }
}
