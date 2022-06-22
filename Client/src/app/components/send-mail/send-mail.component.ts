import { Component, OnInit } from "@angular/core"
import { Router } from "@angular/router"
import { IntakeService } from "src/app/services/intake.service"

@Component({
	selector: "app-send-mail",
	templateUrl: "./send-mail.component.html",
	styleUrls: ["./send-mail.component.css"],
})
export class SendMailComponent implements OnInit {
	reasons = []
	form = {
		reason: { value: 0, invalid: false },
		description: { value: "", invalid: false },
	}

	constructor(private intakeSrv: IntakeService, private router: Router) {}

	async ngOnInit(): Promise<void> {
		this.reasons = await this.intakeSrv.reasons()
	}

	goBack() {
		this.router.navigate(["/intake"])
	}

	validateReason() {
		this.form.reason.invalid = this.form.reason.value == 0
	}
	validateForm(): boolean {
		this.validateReason()
		if (this.form.reason.invalid) {
			return false
		}
		return true
	}

	async sendMail() {
		if (!this.validateForm()) return

		const result = await this.intakeSrv.mail({
			reasonId: +this.form.reason.value,
			description: this.form.description.value,
		})
		if (result) {
			await this.intakeSrv.saveOrder(this.form.description.value, +this.form.reason.value, () => {
				this.goBack()
			})
		}
	}
}
