import { Component, Input, OnDestroy, OnInit } from "@angular/core"

@Component({
	selector: "app-timer",
	templateUrl: "./timer.component.html",
	styleUrls: ["./timer.component.css"],
})
export class TimerComponent implements OnInit, OnDestroy {
	countDownInterval = null
	durationInterval = null
	stroke: string = "283"
	percentage: number = 0
	color: string = "elapsed"
	label: string = "_ : _"
	duration: number = 0
	left: number = 0
	@Input() type = null
	@Input() order = null

	constructor() {}

	ngOnInit(): void {
		if (this.type == "countdown") this.setCountDownTimer()
		else if (this.type == "duration") this.setDurationTimer()
	}

	ngOnDestroy() {
		this.clearCountDownTimer()
		this.clearDurationTimer()
	}

	clearCountDownTimer() {
		if (this.countDownInterval) clearInterval(this.countDownInterval)
		this.color = "elapsed"
		this.label = "_ : _"
		this.duration = 0
		this.left = 0
	}

	clearDurationTimer() {
		if (this.durationInterval) clearInterval(this.durationInterval)
		this.color = "elapsed"
		this.label = "_ : _"
		this.duration = 0
		this.left = 0
	}

	setCountDownTimer() {
		if (this.order == null || this.order.statusCode != "A" || this.order.created == null || this.order.atentionTime <= 0) {
			this.clearCountDownTimer()
			return
		}

		let dateNow: any = new Date()
		let dateStart: any = new Date(this.order.lastUpdate)

		let timePassed = (dateNow - dateStart) / 1000
		this.duration = 60 * this.order.atentionTime
		if (timePassed >= this.duration) {
			this.onCountDownTimesUp()
			return
		}

		this.left = this.duration - timePassed
		if (!this.countDownInterval) this.beginCountDownTimer()
	}

	setDurationTimer() {
		if (this.order == null || this.order.created == null) {
			this.clearDurationTimer()
			return
		}

		let dateStart: any = new Date(this.order.created)
		if (!["P", "A"].includes(this.order.statusCode)) {
			let dateEnd: any = new Date(this.order.lastUpdate)
			this.left = (dateEnd - dateStart) / 1000
			this.percentage = 0
			this.color = "elapsed"
			this.formatTime()
		} else {
			let dateNow: any = new Date()
			this.left = (dateNow - dateStart) / 1000
			if (!this.durationInterval) this.beginDurationTimer()
		}
	}

	onCountDownTimesUp() {
		if (this.countDownInterval) clearInterval(this.countDownInterval)
		this.label = "00:00"
		this.color = "danger"
	}

	beginCountDownTimer() {
		this.countDownInterval = setInterval(() => {
			if (this.left < 0) return
			this.setCountDownTimer()
			this.formatTime()
			this.setCircleDasharray()
			this.setColor()

			if (this.left <= 0) {
				this.onCountDownTimesUp()
			}
		}, 1000)
	}

	beginDurationTimer() {
		this.durationInterval = setInterval(() => {
			if (this.left < 0) return
			this.setDurationTimer()
			this.formatTime()
			if (this.left <= 0) {
				this.onCountDownTimesUp()
			}
		}, 1000)
	}

	formatTime() {
		const days = Math.floor(this.left / (3600 * 24))
		const hours = Math.floor((this.left % (3600 * 24)) / 3600)
		const minutes = Math.floor((this.left % 3600) / 60)
		const seconds = Math.floor(this.left % 60)

		let display = seconds < 10 ? `0${seconds}` : `${seconds}`
		display = minutes < 10 ? `0${minutes}:${display}` : `${minutes}:${display}`
		if (hours > 0) display = hours < 10 ? `0${hours}:${display}` : `${hours}:${display}`
		if (days > 0) display = `${days}d ${display}`

		this.label = display
	}

	setColor() {
		if (this.left <= 0.25 * this.duration) {
			this.color = "danger"
		} else if (this.left <= 0.5 * this.duration) {
			this.color = "warning"
		} else {
			this.color = "success"
		}
	}

	calculateTimeFraction() {
		const rawTimeFraction = this.left / this.duration
		return rawTimeFraction - (1 / this.duration) * (1 - rawTimeFraction)
	}

	setCircleDasharray() {
		//this.percentage = this.left * 100 / this.duration
		this.stroke = `${this.calculateTimeFraction() * 283} 283`
	}
}
