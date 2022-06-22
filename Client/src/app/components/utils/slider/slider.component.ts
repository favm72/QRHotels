import { Component, Input, OnDestroy, OnInit, Output, EventEmitter } from "@angular/core"

@Component({
	selector: "app-slider",
	templateUrl: "./slider.component.html",
	styleUrls: ["./slider.component.css"],
})
export class SliderComponent implements OnInit, OnDestroy {
	@Input() pictures = []
	@Output() slide: EventEmitter<any> = new EventEmitter()
	@Output() click: EventEmitter<any> = new EventEmitter()

	slideIndex: number = 0
	interval = null

	constructor() {}

	ngOnDestroy(): void {
		if (this.interval) clearInterval(this.interval)
	}

	ngOnInit(): void {
		if (this.pictures.length > 1) {
			this.interval = setInterval(() => {
				this.next()
			}, 5000)
		}
	}

	next() {
		this.slideIndex = this.slideIndex == this.pictures.length - 1 ? 0 : this.slideIndex + 1
		this.slide.emit(this.slideIndex)
	}
	prev() {
		this.slideIndex = this.slideIndex == 0 ? this.pictures.length - 1 : this.slideIndex - 1
		this.slide.emit(this.slideIndex)
	}
	current(n: number) {
		this.slideIndex = n
		this.slide.emit(this.slideIndex)
	}
	clickImage(url: string) {
		if (url && url.length > 0) this.click.emit(url)
	}
}
