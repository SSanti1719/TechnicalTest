import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-data-loader',
  standalone: true,
  imports: [CommonModule, MatProgressSpinnerModule, MatIconModule],
  templateUrl: './data-loader.component.html',
  styleUrls: ['./data-loader.component.css'],
})
export class DataLoaderComponent {
  @Input() loading = false;
  @Input() hasError = false;
  @Input() isEmpty = false;
}
